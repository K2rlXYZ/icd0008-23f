using DAL;
using Domain;

namespace UnoEngine;

public class GameEngine
{
    private IGameRepository? Repository { get; set; }
    public GameState State { get; private set; } = new();

    private const int InitialHandSize = 7;
    
    private readonly Random _rnd = new();

    public GameEngine(IGameRepository? repository)
    {
        InitializeDefaultPlayers();
        InitializeFullShuffledDeck();
        DealCardsToPlayers();
        Repository = repository;
        State.Id = Guid.NewGuid();
    }

    private void InitializeDefaultPlayers()
    {
        State.Players = new()
        {
            new Player
            {
                Nickname = "Puny human",
                Type = EPlayerType.Human
            },
            new Player
            {
                Nickname = "Mighty AI",
                Type = EPlayerType.AI
            }
        };
    }

    private List<UnoCard> ShuffleDeck(List<UnoCard> deck)
    {
        var tempShuffledDeck = new List<UnoCard>();

        while (deck.Any())
        {
            int randPos = _rnd.Next(deck.Count);
            tempShuffledDeck.Add(deck[randPos]);
            deck.RemoveAt(randPos);
        }

        return tempShuffledDeck;
    }

    private void InitializeFullShuffledDeck()
    {
        State.DrawDeck.Clear();
        //0 up to +2
        for (int cardValue = 0; cardValue <= (int)EUnoCards.UnoCardsValues.ValuePlus2; cardValue++)
        {
            for (int cardColor = 0; cardColor <= (int)EUnoCards.UnoCardsColors.Blue; cardColor++)
            {
                State.DrawDeck.Add(new UnoCard
                {
                    Value = (EUnoCards.UnoCardsValues)cardValue,
                    Color = (EUnoCards.UnoCardsColors)cardColor
                });
            }
        }
        //1 up to +2
        for (int cardValue = 1; cardValue <= (int)EUnoCards.UnoCardsValues.ValuePlus2; cardValue++)
        {
            for (int cardColor = 0; cardColor <= (int)EUnoCards.UnoCardsColors.Blue; cardColor++)
            {
                State.DrawDeck.Add(new UnoCard
                {
                    Value = (EUnoCards.UnoCardsValues)cardValue,
                    Color = (EUnoCards.UnoCardsColors)cardColor
                });
            }
        }
        //4 of each special card
        for (int amount = 0; amount < 4; amount++)
        {
            for (int cardValue = (int)EUnoCards.UnoCardsValues.ValuePickColor; cardValue <= (int)EUnoCards.UnoCardsValues.ValuePlus4; cardValue++)
            {
                State.DrawDeck.Add(new UnoCard
                {
                    Value = (EUnoCards.UnoCardsValues)cardValue
                });
            }
        }

        State.DrawDeck = ShuffleDeck(State.DrawDeck);

        var firstCard = State.DrawDeck.Last();
        if (firstCard.Value == EUnoCards.UnoCardsValues.ValuePlus4 ||
            firstCard.Value == EUnoCards.UnoCardsValues.ValuePickColor)
        {
            firstCard.Color = (EUnoCards.UnoCardsColors)_rnd.Next((int)EUnoCards.UnoCardsColors.Blue);
        }
        State.PlayDeck.Add(firstCard);
        State.DrawDeck.RemoveAt(State.DrawDeck.Count-1);
    }

    private void DealCardsToPlayers()
    {
        foreach (var player in State.Players)
        {
            for (int deckIndex = 0; deckIndex < InitialHandSize; deckIndex++)
            {
                player.CardsHand.Add(State.DrawDeck.Last());
                State.DrawDeck.RemoveAt(State.DrawDeck.Count-1);
            }
        }
    }

    public void SetAllPlayersHuman()
    {
        foreach (var player in State.Players)
        {
            player.Type = EPlayerType.Human;
        }
        ResetPlayerNicknames();
    }
    
    public void SetAllPlayersAI()
    {
        foreach (var player in State.Players)
        {
            player.Type = EPlayerType.AI;
        }
        ResetPlayerNicknames();
    }
    
    public void SetAllButOnePlayerHuman()
    {
        State.Players[0].Type = EPlayerType.AI;
        for (int playerIndex = 1; playerIndex < State.Players.Count; playerIndex++)
        {
            State.Players[playerIndex].Type = EPlayerType.Human;
        }
        ResetPlayerNicknames();
    }
    
    public void SetAllButOnePlayerAI()
    {
        State.Players[0].Type = EPlayerType.Human;
        for (int playerIndex = 1; playerIndex < State.Players.Count; playerIndex++)
        {
            State.Players[playerIndex].Type = EPlayerType.AI;
        }
        ResetPlayerNicknames();
    }

    private void ResetPlayerNicknames()
    {
        int i = 0;
        foreach (var player in State.Players)
        {
            if (player.Type == EPlayerType.Human)
            {
                player.Nickname = "Human " + (i+1);
                i++;
            }
        }
        i = 0;
        foreach (var player in State.Players)
        {
            if (player.Type == EPlayerType.AI)
            {
                player.Nickname = "AI " + (i+1);
                i++;
            }
        }
    }

    public void SetPlayerCount(int count)
    {
        State.Players.Clear();
        for (int i = 0; i < count; i++)
        {
            State.Players.Add(new Player()
            {
                Nickname = "Human " + (i+1),
                Type = EPlayerType.Human
            });
        }
        InitializeFullShuffledDeck();
        DealCardsToPlayers();
    }

    public string GetPlayerTypes()
    {
        return string.Join(", ",State.Players.Select(player => player.Type).Distinct());
    }

    public string GetPlayerCount()
    {
        return State.Players.Count.ToString();
    }

    public string GetPlayerNicknames()
    {
        return string.Join(", ", State.Players.Select(player => player.Nickname));
    }

    public void SaveGame(Guid? saveName)
    {
        Repository?.SaveGame(saveName, State);
    }

    public void LoadGame(Guid saveName)
    {
        State = Repository?.LoadGameState(saveName) ??
                throw new InvalidOperationException("Loading save: " + saveName + " returned null");
    }

    private void RestockPlayDeckIfEmpty()
    {
        if (!State.DrawDeck.Any())
        {
            var lastCard = State.PlayDeck.Last();
            State.PlayDeck.RemoveAt(State.PlayDeck.Count-1);
            State.DrawDeck = ShuffleDeck(State.PlayDeck);
            State.PlayDeck.Clear();
            State.PlayDeck.Add(lastCard);
        }
    }
    
    private bool CardCanBePlayed(UnoCard card)
    {
        if (card.Color == State.PlayDeck.Last().Color || 
            (card.Value == EUnoCards.UnoCardsValues.ValuePlus4 && 
            !State.Players[State.SelectedPlayerIndex].CardsHand.Select(unoCard => unoCard.Color).Distinct().Contains(State.PlayDeck.Last().Color)) ||
            card.Value == EUnoCards.UnoCardsValues.ValuePickColor)
        {
            return true;
        }
        if (card.Value == State.PlayDeck.Last().Value)
        {
            return true;
        }
        return false;
    }

    private void DrawCard(int playerIndex)
    {
        RestockPlayDeckIfEmpty();
        var card = State.DrawDeck.Last();
        State.Players[playerIndex].CardsHand.Add(card);
        State.Players[playerIndex].Drew = true;
        State.DrawDeck.RemoveAt(State.DrawDeck.Count - 1);
        Console.WriteLine($"{State.Players[playerIndex].Nickname} drew a card");
        //Console.WriteLine($"{State.Players[playerIndex].Nickname} drew a " + drawnCard);
        //Console.ForegroundColor = ConsoleColor.White;
    }

    private bool ApplyCardRules(UnoCard card)
    {
        switch (card.Value)
        {
            case EUnoCards.UnoCardsValues.ValueBlock:
                IncreasePlayerIndex();
                return false;
            case EUnoCards.UnoCardsValues.ValuePlus2:
                for (int i = 0; i < 2; i++)
                {
                    DrawCard(GetNextPlayerIndex());
                }
                IncreasePlayerIndex();
                return false;
            case EUnoCards.UnoCardsValues.ValueReverse:
                State.Direction = -State.Direction;
                return false;
            case EUnoCards.UnoCardsValues.ValuePlus4:
                for (int i = 0; i < 4; i++)
                {
                    DrawCard(GetNextPlayerIndex());
                }
                IncreasePlayerIndex();
                return true;
            case EUnoCards.UnoCardsValues.ValuePickColor:
                return true;
        }
        return false;
    }

    private string? PlayCard(int playerIndex, int cardIndex)
    {
        var card = State.Players[playerIndex].CardsHand[cardIndex];
        if (CardCanBePlayed(card))
        {
            Console.WriteLine(State.Players[playerIndex].Nickname + " PLAYED: " + card);
            Console.ForegroundColor = ConsoleColor.White;
            var askForColor = ApplyCardRules(card);
            State.PlayDeck.Add(card);
            State.Players[playerIndex].CardsHand.RemoveAt(cardIndex);
            IncreasePlayerIndex();
            if (State.Players[playerIndex].CardsHand.Count == 0)
            {
                Console.WriteLine(State.Players[playerIndex].Nickname + " Won");
                return "won";
            }
            if (askForColor)
            {
                // TODO: this isn't nice, but changing it would mean a rework of the whole menu system :(
                return "askForColor";
            }
        }
        else
        {
            Console.WriteLine(card + " Can't be played onto " + State.PlayDeck.Last());
        }
        return null;
    }

    public bool DrawUntilPlayableCard()
    {
        bool drew = false;
        var playableCards = State.Players[State.SelectedPlayerIndex].CardsHand.FindAll(CardCanBePlayed);
        while (playableCards.Count == 0)
        {
            DrawCard(State.SelectedPlayerIndex);
            playableCards = State.Players[State.SelectedPlayerIndex].CardsHand.FindAll(CardCanBePlayed);
            drew = true;
        }
        return drew;
    }
    
    public string? AIPlayRandomCard()
    {
        DrawUntilPlayableCard();
        var playableCards = State.Players[State.SelectedPlayerIndex].CardsHand.FindAll(CardCanBePlayed);
        var randomCard = playableCards[_rnd.Next(playableCards.Count)];
        var result = PlayCard(State.SelectedPlayerIndex, State.Players[State.SelectedPlayerIndex].CardsHand.IndexOf(randomCard));
        if (result == "askForColor")
        {
            State.PlayDeck.Last().Color=(EUnoCards.UnoCardsColors)_rnd.Next((int)EUnoCards.UnoCardsColors.Blue);
            return result;
        }

        if (result == "won") return null;
        return result;
    }
    
    public string? HumanPlayCard(int playerIndex, int cardIndex)
    {
        return PlayCard(playerIndex, cardIndex);
    }

    private int GetNextPlayerIndex()
    {
        var tempIndex = State.SelectedPlayerIndex;
        tempIndex+=State.Direction;
        if (tempIndex > State.Players.Count - 1)
        {
            return tempIndex - State.Players.Count;
        }
        if (tempIndex < 0)
        {
            return tempIndex + State.Players.Count;
        }

        return tempIndex;
    }
    
    public int GetLastPlayerIndex(int moveTimes)
    {
        var tempIndex = State.SelectedPlayerIndex;
        for (int i = 0; i < moveTimes; i++)
        {
            tempIndex-=State.Direction;
            if (tempIndex > State.Players.Count - 1)
            {
                return tempIndex - State.Players.Count;
            }
            if (tempIndex < 0)
            {
                return tempIndex + State.Players.Count;
            }
        }

        return tempIndex;
    }

    public void IncreasePlayerIndex()
    {
        GetActivePlayer().Drew = false;
        State.SelectedPlayerIndex = GetNextPlayerIndex();
    }

    public void SetTopCardColor(string? color)
    {
        EUnoCards.UnoCardsColors eColor;
        switch (color)
        {
            case "y":
                eColor = EUnoCards.UnoCardsColors.Yellow;
                break;
            case "r":
                eColor = EUnoCards.UnoCardsColors.Red;
                break;
            case "l":
                eColor = EUnoCards.UnoCardsColors.Blue;
                break;
            case "g":
                eColor = EUnoCards.UnoCardsColors.Green;
                break;
            default:
                Console.WriteLine("Undefined option");
                return;
        }
        State.PlayDeck.Last().Color = eColor;
    }

    public Player GetActivePlayer()
    {
        return State.Players[State.SelectedPlayerIndex];
    }
}