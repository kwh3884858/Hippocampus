#ifndef StoryLayoutConstant_h
#define StoryLayoutConstant_h

#include "HeavenGateEditorConstant.h"

namespace HeavenGateEditor
{

    enum class TableType
    {
        None = 0,
        Font_Size,
        Color,
        Tips,
        TachieMove,
//        Chapter,
//        Scene,
        Character,
        Pause,
        Exhibit,
        Effect,
        Bgm,
        Tachie,
        Tachie_Position
    };

    int MappingLayoutToArrayIndex(int layout);
    const char * GetLayoutString(TableType type);
    int GetLayoutAmount(TableType type);

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

    enum class TachieMoveTableLayout
    {
        Type = 0,
        MoveAlias,
        TachieName,
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
        ExhibitID,
        Exhibit,
        Description,
        ExhibitImageName,

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
        FileName = 2,
        Volume,

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
        "tachieName",
        "startPointAlias",
        "endPointAlias",
        "moveCurve",
        "duration"

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
