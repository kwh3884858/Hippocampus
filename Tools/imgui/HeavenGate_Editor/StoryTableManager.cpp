

#include "StoryTableManager.h"

#include "StoryTable.h"

namespace HeavenGateEditor {

    bool StoryTableManager::Initialize()
    {
        m_colorTable = new StoryTable<COLOR_MAX_COLUMN>;
        m_colorTable->SetTableType(TableType::Color);

        m_colorTable->PushName("Color name");
        m_colorTable->PushName("RGB Value");


        m_fontSizeTable = new StoryTable< FONT_SIZE_MAX_COLUMN>;
        m_fontSizeTable->SetTableType(TableType::Font_Size);

        m_fontSizeTable->PushName("Size Key");
        m_fontSizeTable->PushName("Font Size Value");


        m_tipTable = new StoryTable<TIP_MAX_COLUMN>;
        m_tipTable->SetTableType(TableType::Tips);

        m_tipTable->PushName("Tip");
        m_tipTable->PushName("Description");



        m_paintMovetable = new StoryTable<PAINT_MOVE_MAX_COLUMN>;
        m_paintMovetable->SetTableType(TableType::Paint_Move);

        m_paintMovetable->PushName("Angle Value");
        m_paintMovetable->PushName("Move Distance");
        return true;

    }

    bool StoryTableManager::Shutdown()
    {
        delete m_colorTable;
        m_colorTable = nullptr;

        delete m_fontSizeTable;
        m_fontSizeTable = nullptr;

        delete m_tipTable;
        m_tipTable = nullptr;

        delete m_paintMovetable;
        m_paintMovetable = nullptr;

        return true;
    }

    const StoryTable<HeavenGateEditor::COLOR_MAX_COLUMN>* const StoryTableManager::GetColorTable() const
    {
        return m_colorTable;
    }

    StoryTable<COLOR_MAX_COLUMN>* StoryTableManager::GetColorTable()
    {
        return const_cast<StoryTable<COLOR_MAX_COLUMN>*>(const_cast<const StoryTableManager*>(this)->GetColorTable());
    }

    const StoryTable<FONT_SIZE_MAX_COLUMN>* const StoryTableManager::GetFontSizeTable() const
    {
        return m_fontSizeTable;

    }

    StoryTable<FONT_SIZE_MAX_COLUMN>* StoryTableManager::GetFontSizeTable()
    {
        return const_cast<StoryTable<FONT_SIZE_MAX_COLUMN>*>(const_cast<const StoryTableManager*>(this)->GetFontSizeTable());

    }

    const StoryTable<TIP_MAX_COLUMN>* const StoryTableManager::GetTipTable() const
    {
        return m_tipTable;

    }

    StoryTable<TIP_MAX_COLUMN>* StoryTableManager::GetTipTable()
    {
        return const_cast<StoryTable<TIP_MAX_COLUMN>*>(const_cast<const StoryTableManager*>(this)->GetTipTable());
    }

    const StoryTable<PAINT_MOVE_MAX_COLUMN>* const StoryTableManager::GetPaintMoveTable() const
    {
        return m_paintMovetable;
    }

    StoryTable<PAINT_MOVE_MAX_COLUMN>* StoryTableManager::GetPaintMoveTable()
    {
        return const_cast<StoryTable<PAINT_MOVE_MAX_COLUMN>*>(const_cast<const StoryTableManager*>(this)->GetPaintMoveTable());

    }

}
