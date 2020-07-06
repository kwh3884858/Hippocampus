
namespace HeavenGateEditor {

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


enum class TableLayout {
    Type = 0,
    Header = 1,
    Value = 2
};

enum class FontSizeTableLayout {
    Type = 0,
    Alias = 1,
    Size = 2
};

enum class ColorTableLayout {
    Type = 0,
    Alias = 1,
    //Size = 2
    R,
    G,
    B,
    A
};

enum class TipTableLayout {
    Type = 0,
    Tip = 1,
    Description = 2
};

enum class PaintMoveTableLayout {
    Type = 0,
    MoveAlias = 1,
    StartPoint = 2,
    EndPoint = 3,
    MoveType = 4
};

enum class ChapterTableLayout {
    Type = 0,
    Chapter = 1,
    Description = 2
};

enum class SceneTableLayout {
    Type = 0,
    Scene = 1,
    Description = 2
};

enum class CharacterTableLayout {
    Type = 0,
    Character = 1,
    Description = 2
};

enum class PauseTableLayout {
    Type = 0,
    Pause = 1,
    Time = 2
};

enum class ExhibitTableLayout {
    Type = 0,
    Exhibit = 1,
    Description = 2
};

enum class EffectTableLayout {
    Type = 0,
    Effect = 1,
    Description = 2
};

enum class BgmTableLayout {
    Type = 0,
    Bgm = 1,
    Description = 2
};

enum class TachieTableLayout {
    Type = 0,
    Alias = 1,
    FileName = 2
};

enum class TachiePositionTableLayout{
    Type = 0,
    Alias = 1,
    PositionX = 2,
    PositionY = 3
};

const char TableTypeString[][MAX_ENUM_LENGTH] ={
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
    "Tachie_Position"
};


const char tableString[][MAX_ENUM_LENGTH] = {
    "type",
    "header",
    "value"
};

const char fontSizeTableString[][MAX_ENUM_LENGTH] = {
    "fontSize",
    "alias",
    "size"
};

const char colorTableString[][MAX_ENUM_LENGTH] = {
    "color",
    "alias",
    //"size"
    "r",
    "g",
    "b",
    "a"
};

const char tipTableString[][MAX_ENUM_LENGTH] = {
    "tip",
    "tip",
    "description"
};

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
    "roleDrawing",
    "alias",
    "fileName"
};

const char tachiePositionTableString[][MAX_ENUM_LENGTH] = {
    "tachiePosition",
    "alias",
    "positionX",
    "positionY"
};

}
