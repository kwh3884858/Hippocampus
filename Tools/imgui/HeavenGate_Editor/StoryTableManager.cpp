

#include "StoryTableManager.h"

#include "StoryTable.h"

namespace HeavenGateEditor {

    bool StoryTableManager::Initialize()
    {
        m_colorTable = new StoryTable<COLOR_MAX_COLUMN>;
        m_colorTable->SetTableType(TableType::Color);

//        m_colorTable->PushName("Color name");
//        m_colorTable->PushName("RGB Value");

        m_fontSizeTable = new StoryTable< FONT_SIZE_MAX_COLUMN>;
        m_fontSizeTable->SetTableType(TableType::Font_Size);

//        m_fontSizeTable->PushName("Size Key");
//        m_fontSizeTable->PushName("Font Size Value");

        m_tipTable = new StoryTable<TIP_MAX_COLUMN, TIP_TABLE_MAX_CONTENT>;
        m_tipTable->SetTableType(TableType::Tips);

//        m_tipTable->PushName("Tip");
//        m_tipTable->PushName("Description");

        m_paintMovetable = new StoryTable<TACHIE_MOVE_MAX_COLUMN>;
        m_paintMovetable->SetTableType(TableType::TachieMove);

//        m_paintMovetable->PushName("Angle Value");
//        m_paintMovetable->PushName("Move Distance");

        //m_chapterTable = new StoryTable<CHAPTER_COLUMN>;
        //m_chapterTable->SetTableType(TableType::Chapter);

//        m_chapterTable->PushName("Chapter");
//        m_chapterTable->PushName("Description");

        //m_sceneTable = new StoryTable<SCENE_COLUMN>;
        //m_sceneTable->SetTableType(TableType::Scene);

//        m_sceneTable->PushName("Scene");
//        m_sceneTable->PushName("Description");

        m_characterTable = new StoryTable<CHARACTER_COLUMN>;
        m_characterTable->SetTableType(TableType::Character);

//        m_characterTable->PushName("Character");
//        m_characterTable->PushName("Description");

        m_pauseTable = new StoryTable<PAUSE_MAX_COLUMN>;
        m_pauseTable->SetTableType(TableType::Pause);

//        m_pauseTable->PushName("Pause");
//        m_pauseTable->PushName("Time");

        m_exhibitTable = new StoryTable<EXHIBIT_COLUMN, EXHIBIT_TABLE_MAX_CONTENT>;
        m_exhibitTable->SetTableType(TableType::Exhibit);

//        m_exhibitTable->PushName("Exhibit");
//        m_exhibitTable->PushName("Description");

        m_effectTable = new StoryTable<EFFECT_MAX_COLUMN>;
        m_effectTable->SetTableType(TableType::Effect);

//        m_effectTable->PushName("Effect");
//        m_effectTable->PushName("Description");

        m_bgmTable = new StoryTable<BGM_MAX_COLUMN>;
        m_bgmTable->SetTableType(TableType::Bgm);

//        m_bgmTable->PushName("Bgm");
//        m_bgmTable->PushName("Description");

        m_tachieTable = new StoryTable<TACHIE_MAX_COLUMN>;
        m_tachieTable->SetTableType(TableType::Tachie);

//        m_tachieTable->PushName("Tachie");
//        m_tachieTable->PushName("File Name");

        m_tachiePositionTable = new StoryTable<TACHIE_POSITION_MAX_COLUMN>;
        m_tachiePositionTable->SetTableType(TableType::Tachie_Position);

//        m_tachiePositionTable->PushName("Alias");
//        m_tachiePositionTable->PushName("Pos X");
//        m_tachiePositionTable->PushName("Pos Y");

        m_positionTable = new StoryTable<POSITION_MAX_COLUMN>;
        m_positionTable->SetTableType(TableType::Position);

        m_rotationTable = new StoryTable<ROTATION_MAX_COLUMN>;
        m_rotationTable->SetTableType(TableType::Rotation);

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

        //delete m_chapterTable;
        //m_chapterTable = nullptr;

        //delete m_sceneTable;
        //m_sceneTable = nullptr;

        delete m_characterTable;
        m_characterTable = nullptr;

        delete m_pauseTable;
        m_pauseTable = nullptr;

        delete m_exhibitTable;
        m_exhibitTable = nullptr;

        delete m_effectTable;
        m_effectTable = nullptr;

        delete m_bgmTable;
        m_bgmTable = nullptr;

        delete m_tachieTable;
        m_tachieTable = nullptr;

        delete m_tachiePositionTable;
        m_tachiePositionTable = nullptr;

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

    const StoryTable<TIP_MAX_COLUMN, TIP_TABLE_MAX_CONTENT>* const StoryTableManager::GetTipTable() const
    {
        return m_tipTable;

    }

    StoryTable<TIP_MAX_COLUMN, TIP_TABLE_MAX_CONTENT>* StoryTableManager::GetTipTable()
    {
        return const_cast<StoryTable<TIP_MAX_COLUMN, TIP_TABLE_MAX_CONTENT>*>(const_cast<const StoryTableManager*>(this)->GetTipTable());
    }

    const StoryTable<TACHIE_MOVE_MAX_COLUMN>* const StoryTableManager::GetPaintMoveTable() const
    {
        return m_paintMovetable;
    }

    StoryTable<TACHIE_MOVE_MAX_COLUMN>* StoryTableManager::GetPaintMoveTable()
    {
        return const_cast<StoryTable<TACHIE_MOVE_MAX_COLUMN>*>(const_cast<const StoryTableManager*>(this)->GetPaintMoveTable());

    }

    //const StoryTable<CHAPTER_COLUMN>* const StoryTableManager::GetChapterTable() const
    //{
    //    return m_chapterTable;
    //}

    //StoryTable<CHAPTER_COLUMN>* StoryTableManager::GetChapterTable()
    //{
    //    return const_cast<StoryTable<CHAPTER_COLUMN>*>(const_cast<const StoryTableManager*>(this)->GetChapterTable());

    //}

    //const StoryTable<SCENE_COLUMN>* const StoryTableManager::GetSceneTable() const
    //{
    //    return m_sceneTable;
    //}

    //StoryTable<SCENE_COLUMN>* StoryTableManager::GetSceneTable()
    //{
    //    return const_cast<StoryTable<SCENE_COLUMN>*>(const_cast<const StoryTableManager*>(this)->GetSceneTable());

    //}

    const StoryTable<CHARACTER_COLUMN>* const StoryTableManager::GetCharacterTable() const
    {
        return m_characterTable;
    }

    StoryTable<CHARACTER_COLUMN>* StoryTableManager::GetCharacterTable()
    {
        return const_cast<StoryTable<CHARACTER_COLUMN>*>(const_cast<const StoryTableManager*>(this)->GetCharacterTable());

    }

    const StoryTable<PAUSE_MAX_COLUMN>* const StoryTableManager::GetPauseTable() const
    {
        return m_pauseTable;
    }

    StoryTable<PAUSE_MAX_COLUMN>* StoryTableManager::GetPauseTable()
    {
        return const_cast<StoryTable<PAUSE_MAX_COLUMN>*>(const_cast<const StoryTableManager*>(this)->GetPauseTable());

    }

    const StoryTable<EXHIBIT_COLUMN, EXHIBIT_TABLE_MAX_CONTENT>* const StoryTableManager::GetExhibitTable() const
    {
        return m_exhibitTable;
    }

    StoryTable<EXHIBIT_COLUMN, EXHIBIT_TABLE_MAX_CONTENT>* StoryTableManager::GetExhibitTable()
    {
        return const_cast<StoryTable<EXHIBIT_COLUMN, EXHIBIT_TABLE_MAX_CONTENT>*>(const_cast<const StoryTableManager*>(this)->GetExhibitTable());

    }

    const StoryTable<EFFECT_MAX_COLUMN>* const StoryTableManager::GetEffectTable() const
    {
        return m_effectTable;
    }

    StoryTable<EFFECT_MAX_COLUMN>* StoryTableManager::GetEffectTable()
    {
        return const_cast<StoryTable<EFFECT_MAX_COLUMN>*>(const_cast<const StoryTableManager*>(this)->GetEffectTable());

    }

    const StoryTable<BGM_MAX_COLUMN>* const StoryTableManager::GetBgmTable() const
    {
        return m_bgmTable;
    }

    StoryTable<BGM_MAX_COLUMN>* StoryTableManager::GetBgmTable()
    {
        return const_cast<StoryTable<BGM_MAX_COLUMN>*>(const_cast<const StoryTableManager*>(this)->GetBgmTable());

    }

    const StoryTable<TACHIE_MAX_COLUMN>* const StoryTableManager::GetTachieTable() const
    {
        return m_tachieTable;
    }

    StoryTable<TACHIE_MAX_COLUMN>* StoryTableManager::GetTachieTable()
    {
        return const_cast<StoryTable<TACHIE_MAX_COLUMN>*>(const_cast<const StoryTableManager*>(this)->GetTachieTable());

    }

    const StoryTable<TACHIE_POSITION_MAX_COLUMN>* const StoryTableManager::GetTachiePositionTable() const
    {
        return m_tachiePositionTable;
    }

    StoryTable<TACHIE_POSITION_MAX_COLUMN>*  StoryTableManager::GetTachiePositionTable()
    {
        return const_cast<StoryTable<TACHIE_POSITION_MAX_COLUMN>*>(const_cast<const StoryTableManager*>(this)->GetTachiePositionTable());
    }

    const StoryTable<POSITION_MAX_COLUMN>* const StoryTableManager::GetPositionTable() const
    {
        return m_positionTable;
    }

    StoryTable<POSITION_MAX_COLUMN>* StoryTableManager::GetPositionTable()
    {
        return const_cast<StoryTable<POSITION_MAX_COLUMN>*>(const_cast<const StoryTableManager*>(this)->GetPositionTable());
    }

    const StoryTable<ROTATION_MAX_COLUMN>* const StoryTableManager::GetRotationTable() const
    {
        return m_rotationTable;
    }
    StoryTable<ROTATION_MAX_COLUMN>* StoryTableManager::GetRotationTable()
    {
        return const_cast<StoryTable<ROTATION_MAX_COLUMN>*>(const_cast<const StoryTableManager*>(this)->GetRotationTable());
    }
}
