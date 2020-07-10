#ifndef StoryLayoutConstant_h
#define StoryLayoutConstant_h

namespace HeavenGateEditor
{

    enum class TableType
    {
        None = 0,
        Font_Size,
        Color,
        Tips,
        Paint_Move,
        Chapter,
        Scene,
        Character,
        Pause,
        Exhibit,
        Effect,
        Bgm,
        Tachie,
        Tachie_Position
    };

    enum class TableLayout
    {
        Type = 0,
        Header = 1,
        Value = 2
    };

    enum class FontSizeTableLayout
    {
        Type = 0,
        Alias = 1,
        Size = 2,

        Amount
    };

    enum class ColorTableLayout
    {
        Type = 0,
        Alias = 1,
        //Size = 2
        R,
        G,
        B,
        A,

        Amount
    };

    enum class TipTableLayout
    {
        Type = 0,
        Tip = 1,
        Description = 2,

        Amount
    };

    enum class PaintMoveTableLayout
    {
        Type = 0,
        MoveAlias = 1,
        StartPoint = 2,
        EndPoint = 3,
        MoveType = 4,

        Amount
    };

    enum class ChapterTableLayout
    {
        Type = 0,
        Chapter = 1,
        Description = 2,

        Amount
    };

    enum class SceneTableLayout
    {
        Type = 0,
        Scene = 1,
        Description = 2,

        Amount
    };

    enum class CharacterTableLayout
    {
        Type = 0,
        Character = 1,
        Description = 2,

        Amount
    };

    enum class PauseTableLayout
    {
        Type = 0,
        Pause = 1,
        Time = 2,

        Amount
    };

    enum class ExhibitTableLayout
    {
        Type = 0,
        Exhibit = 1,
        Description = 2,

        Amount
    };

    enum class EffectTableLayout
    {
        Type = 0,
        Effect = 1,
        Description = 2,

        Amount
    };

    enum class BgmTableLayout
    {
        Type = 0,
        Bgm = 1,
        Description = 2,

        Amount
    };

    enum class TachieTableLayout
    {
        Type = 0,
        Alias = 1,
        FileName = 2,

        Amount
    };

    enum class TachiePositionTableLayout
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
        "Paint_Move",
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

    const char paintMoveTableString[][MAX_ENUM_LENGTH] = {
        "paintMove",
        "moveAlias",
        "startPoint",
        "endPoint",
        "moveType",

    };

    const char chapterTableString[][MAX_ENUM_LENGTH] = {
        "chapter",
        "chapter",
        "description"

    };

    const char sceneTableString[][MAX_ENUM_LENGTH] = {
        "scene",
        "scene",
        "description"

    };

    const char characterTableString[][MAX_ENUM_LENGTH] = {
        "character",
        "character",
        "description"

    };

    const char pauseTableString[][MAX_ENUM_LENGTH] = {
        "pause",
        "pause",
        "time"

    };

    const char exhibitTableString[][MAX_ENUM_LENGTH] = {
        "exhibit",
        "exhibit",
        "description"

    };

    const char effectTableString[][MAX_ENUM_LENGTH] = {
        "effect",
        "effect",
        "description"

    };

    const char bgmTableString[][MAX_ENUM_LENGTH] = {
        "bgm",
        "bgm",
        "description"

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

    const char * GetLayoutString(TableType type)
    {
        switch (type)
        {
        case TableType::None:
            return nullptr;
        case TableType::Font_Size:
            return (const char *)fontSizeTableString;
        case TableType::Color:
            return (const char *)colorTableString;
        case TableType::Tips:
            return (const char *)tipTableString;
        case TableType::Paint_Move:
            return (const char *)paintMoveTableString;
        case TableType::Chapter:
            return (const char *)sceneTableString;
        case TableType::Scene:
            return (const char *)sceneTableString;
        case TableType::Character:
            return (const char *)characterTableString;
        case TableType::Pause:
            return (const char *)pauseTableString;
        case TableType::Exhibit:
            return (const char *)exhibitTableString;
        case TableType::Effect:
            return (const char *)effectTableString;
        case TableType::Bgm:
            return (const char *)bgmTableString;
        case TableType::Tachie:
            return (const char *)tachieTableString;
        case TableType::Tachie_Position:
            return (const char *)tachiePositionTableString;
        default:
            return nullptr;
        }
    }

    int GetLayoutAmount(TableType type)
    {
        switch (type)
        {
        case TableType::None:
            return -1;
        case TableType::Font_Size:
            return (int)FontSizeTableLayout::Amount;
        case TableType::Color:
            return (int)ColorTableLayout::Amount;
        case TableType::Tips:
            return (int)TipTableLayout::Amount;
        case TableType::Paint_Move:
            return (int)PaintMoveTableLayout::Amount;
        case TableType::Chapter:
            return (int)ChapterTableLayout::Amount;
        case TableType::Scene:
            return (int)SceneTableLayout::Amount;
        case TableType::Character:
            return (int)CharacterTableLayout::Amount;
        case TableType::Pause:
            return (int)PauseTableLayout::Amount;
        case TableType::Exhibit:
            return (int)ExhibitTableLayout::Amount;
        case TableType::Effect:
            return (int)EffectTableLayout::Amount;
        case TableType::Bgm:
            return (int)BgmTableLayout::Amount;
        case TableType::Tachie:
            return (int)TachieTableLayout::Amount;
        case TableType::Tachie_Position:
            return (int)TachiePositionTableLayout::Amount;
        default:
            return -1;
        }
    }

} // namespace HeavenGateEditor

#endif /* StoryLayoutConstant_h*/
