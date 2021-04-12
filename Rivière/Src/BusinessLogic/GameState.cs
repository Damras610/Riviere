namespace Rivière.BusinessLogic
{
    public enum GameState
    {
        NOT_STARTED = -2,
        WAITING_FOR_PLAYER = -1,
        WAITING_FOR_START = 0,
        // DO NOT CHANGE THE INTEGER VALUES FROM ASKING_FOR_COLOR
        ASKING_FOR_COLOR = 1,
        ASKING_FOR_LESS_EQUAL_MORE = 2,
        ASKING_FOR_INTER_EQUAL_EXTER = 3,
        ASKING_FOR_SUIT = 4,
        GIVING_OR_RECEIVING_DRINKS = 5,
        // ...TO GIVING_OR_RECEIVING_DRINKS
        FINISHED = 6
    }
}
