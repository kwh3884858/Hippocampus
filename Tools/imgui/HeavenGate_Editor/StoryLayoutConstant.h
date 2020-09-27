#ifndef StoryLayoutConstant_h
#define StoryLayoutConstant_h

namespace HeavenGateEditor
{

    const int     MAX_ENUM_LENGTH = 32;
    enum class TableType
    {
        None = 0,
        Font_Size,
        Color,
        Tips,
        TachieMove,
        Character,
        Pause,
        Exhibit,
        Effect,
        Bgm,
        Tachie,
        Tachie_Position
    };

    constexpr int MappingLayoutToArrayIndex(int layout) {
        return layout - 1;
    }

    const char * GetLayoutString(TableType type);
    int GetLayoutAmount(TableType type);

    enum class TableLayout
    {
        Type = 0,
        Header = 1,
        Value = 2
    };

    enum class FontSizeTableLayout: int
    {
        Type = 0,
        Alias = 1,
        Size = 2,

        Amount
    };

    enum class ColorTableLayout: int
    {
        Type = 0,
        Alias = 1,
        R,
        G,
        B,
        A,

        Amount
    };

    enum class TipTableLayout: int
    {
        Type = 0,
        Tip = 1,
        Description = 2,

        Amount
    };

    enum class TachieMoveTableLayout: int
    {
        Type = 0,
        MoveAlias,
        StartPointAlias,
        EndPointAlias,
        MoveCurve,
        Duration,

        Amount
    };

//    enum class ChapterTableLayout
//    {
//        Type = 0,
//        Chapter = 1,
//        Description = 2,
//
//        Amount
//    };
//
//    enum class SceneTableLayout
//    {
//        Type = 0,
//        Scene = 1,
//        Description = 2,
//
//        Amount
//    };

    enum class CharacterTableLayout: int
    {
        Type = 0,
        CharacterName = 1,
        TachieId = 2,

        Amount
    };

    enum class PauseTableLayout: int
    {
        Type = 0,
        Pause = 1,
        Time = 2,

        Amount
    };

    enum class ExhibitTableLayout: int
    {
        Type = 0,
        ExhibitID,
        Exhibit,
        Description,
        ExhibitImageName,

        Amount
    };

    enum class EffectTableLayout: int
    {
        Type = 0,
        Effect = 1,
        Description = 2,

        Amount
    };

    enum class BgmTableLayout: int
    {
        Type = 0,
        Bgm = 1,
        FileName = 2,
        Volume,

        Amount
    };

    enum class TachieTableLayout : int
    {
        Type = 0,
        Alias = 1,
        FileName = 2,

        Amount
    };

    enum class TachiePositionTableLayout : int
    {
        Type = 0,
        Alias = 1,
        PositionX = 2,
        PositionY = 3,

        Amount
    };

    const char TableTypeString[][MAX_ENUM_LENGTH] = {
        "None",
        "Font_Size",
        "Color",
        "Tips",
        "TachieMove",
        "Chapter",
        "Scene",
        "Character",
        "Pause",
        "Exhibit",
        "Effect",
        "Bgm",
        "Tachie",
        "Tachie_Position"};

    const char tableString[][MAX_ENUM_LENGTH] = {
        "type",
        "header",
        "value"};

    const char fontSizeTableString[][MAX_ENUM_LENGTH] = {
        "fontSize",
        "alias",
        "size"};

    const char colorTableString[][MAX_ENUM_LENGTH] = {
        "color",
        "alias",
        //"size"
        "r",
        "g",
        "b",
        "a"};

    const char tipTableString[][MAX_ENUM_LENGTH] = {
        "tip",
        "tip",
        "description"};

    const char tachieMoveTableString[][MAX_ENUM_LENGTH] = {
        "tachieMove",
        "moveAlias",
        "startPointAlias",
        "endPointAlias",
        "moveCurve",
        "duration"

    };

    const char characterTableString[][MAX_ENUM_LENGTH] = {
        "character",
        "characterName",
        "tachieId"

    };

    const char pauseTableString[][MAX_ENUM_LENGTH] = {
        "pause",
        "pause",
        "time"

    };

    const char exhibitTableString[][MAX_ENUM_LENGTH] = {
        "exhibit",
        "exhibitID",
        "exhibit",
        "description",
        "exhibitImageName"
    };

    const char effectTableString[][MAX_ENUM_LENGTH] = {
        "effect",
        "effect",
        "description"

    };

    const char bgmTableString[][MAX_ENUM_LENGTH] = {
        "bgm",
        "bgm",
        "description",
        "volume"

    };

    const char tachieTableString[][MAX_ENUM_LENGTH] = {
        "tachie",
        "alias",
        "fileName"};

    const char tachiePositionTableString[][MAX_ENUM_LENGTH] = {
        "tachiePosition",
        "alias",
        "positionX",
        "positionY"};


} // namespace HeavenGateEditor

#endif /* StoryLayoutConstant_h*/
