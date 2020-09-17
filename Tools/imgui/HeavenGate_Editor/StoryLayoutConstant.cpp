
#include "StoryLayoutConstant.h"

namespace HeavenGateEditor {

int MappingLayoutToArrayIndex(int layout){
    return layout - 1;
}
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
        case TableType::TachieMove:
            return (const char *)tachieMoveTableString;
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
        case TableType::TachieMove:
            return (int)TachieMoveTableLayout::Amount;
//        case TableType::Chapter:
//            return (int)ChapterTableLayout::Amount;
//        case TableType::Scene:
//            return (int)SceneTableLayout::Amount;
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

}
