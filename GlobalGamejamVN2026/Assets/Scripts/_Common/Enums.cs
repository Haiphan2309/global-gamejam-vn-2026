
namespace GDC.Enums
{
    public enum TransitionType
    {
        NONE,
        LEFT,
        RIGHT,
        UP,
        DOWN,
        IN,
        FADE,
    }
    public enum TransitionLoadSceneType
    {
        NEW_SCENE, //Load sang scene moi
        RELOAD_WITH_TRANSITION, //Load lai scene cu nhung van co transition
        RELOAD_WITHOUT_TRANSITION //Load lai scene cu va khong co transition
    }
    
    public enum SceneType
    {
        UNKNOWN = -999,
        MAIN = 0,
        HOME = 1,
        TUTORIAL = 2,
        INTRO = 3,   
    }

    public enum ItemType
    {
        UNKNOWN = -999,
        FISH = 0,
        DECORATION,
        DROPABLE_OBJECT,
    }

    public enum DecorationType
    {
        UNKNOWN = -999,
        KELP = 0,
        SMALL_ROCK = 10,
    }

    public enum DropableObjectType
    {
        UNKNOWN = -999,
        FISH_FOOD = 0,
        FISH_MEDICINE = 10,
    }

    public enum FishSpecie
    {
        UNKNOWN = -999,
        GOLD_FISH = 0,
        CLOWN_FISH,
        SEA_HORSE,
        JELLY_FISH,
    }

    public enum NPCSpecie
    {
        UNKNOWN = -999,

        //Wandering NPC
        CAT = 0,

        DOG = 10,

        //Soccer NPC
        SOCCER_KID = 20,
    }

    public enum TankType
    {
        UNKNOWN = -999,

        SMALL_TANK = 0,
        NORMAL_TANK
    }

    public enum PlayerAction
    {
        NONE = -999,

        POINT = 0,
        PUSH,
        PET,
        THREATEN,
    }

    public enum CollectibleType
    {
        COIN_10 = 0,
        COIN_20,
        COIN_50,
    }

    public enum Height
    {
        VERT_SHORT,
        SHORT,
        MEDIUM,
        TALL,
        VERY_TALL,
    }
}
