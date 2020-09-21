#pragma once
#ifndef StoryTableManager_h
#define StoryTableManager_h

#include "StorySingleton.h"

#include "HeavenGateEditorConstant.h"
#include "StoryTableDefine.h"

namespace HeavenGateEditor {

    class StoryTableManager final :public StorySingleton<StoryTableManager>
    {
    public:
        StoryTableManager() = default;
        virtual ~StoryTableManager() override = default;

        //initialize function, take the place of constructor
        virtual bool Initialize() override;

        //destroy function, take the  place of destructor
        virtual bool Shutdown() override;


        //Getter
        const StoryTable<COLOR_MAX_COLUMN>* const GetColorTable() const;
        StoryTable<COLOR_MAX_COLUMN>*  GetColorTable();

        const StoryTable<FONT_SIZE_MAX_COLUMN>* const GetFontSizeTable() const;
        StoryTable<FONT_SIZE_MAX_COLUMN>*  GetFontSizeTable();

        const StoryTable<TIP_MAX_COLUMN, TIP_TABLE_MAX_CONTENT>* const GetTipTable() const;
        StoryTable<TIP_MAX_COLUMN, TIP_TABLE_MAX_CONTENT>*  GetTipTable();

        const StoryTable<TACHIE_MOVE_MAX_COLUMN>* const GetPaintMoveTable() const;
        StoryTable<TACHIE_MOVE_MAX_COLUMN>*  GetPaintMoveTable();

        const StoryTable<CHARACTER_COLUMN>* const GetCharacterTable() const;
        StoryTable<CHARACTER_COLUMN>*  GetCharacterTable();

        const StoryTable<PAUSE_MAX_COLUMN>* const GetPauseTable() const;
        StoryTable<PAUSE_MAX_COLUMN>*  GetPauseTable();

        const StoryTable<EXHIBIT_COLUMN, EXHIBIT_TABLE_MAX_CONTENT>* const GetExhibitTable() const;
        StoryTable<EXHIBIT_COLUMN, EXHIBIT_TABLE_MAX_CONTENT>*  GetExhibitTable();

        const StoryTable<EFFECT_MAX_COLUMN>* const GetEffectTable() const;
        StoryTable<EFFECT_MAX_COLUMN>*  GetEffectTable();

        const StoryTable<BGM_MAX_COLUMN>* const GetBgmTable() const;
        StoryTable<BGM_MAX_COLUMN>*  GetBgmTable();

        const StoryTable<TACHIE_MAX_COLUMN>* const GetTachieTable() const;
        StoryTable<TACHIE_MAX_COLUMN>*  GetTachieTable();

        const StoryTable<TACHIE_POSITION_MAX_COLUMN>* const GetTachiePositionTable() const;
        StoryTable<TACHIE_POSITION_MAX_COLUMN>*  GetTachiePositionTable();

    protected:
    private:


        StoryTable<COLOR_MAX_COLUMN>*  m_colorTable;
        StoryTable<FONT_SIZE_MAX_COLUMN>* m_fontSizeTable;
        StoryTable<TIP_MAX_COLUMN, TIP_TABLE_MAX_CONTENT>* m_tipTable;
        StoryTable<TACHIE_MOVE_MAX_COLUMN>* m_paintMovetable;
        //StoryTable<CHAPTER_COLUMN>* m_chapterTable;
        //StoryTable<SCENE_COLUMN>* m_sceneTable;
        StoryTable<CHARACTER_COLUMN>* m_characterTable;
        StoryTable<PAUSE_MAX_COLUMN>* m_pauseTable;
        StoryTable<EXHIBIT_COLUMN, EXHIBIT_TABLE_MAX_CONTENT>* m_exhibitTable;
        StoryTable<EFFECT_MAX_COLUMN>* m_effectTable;
        StoryTable<BGM_MAX_COLUMN>* m_bgmTable;
        StoryTable<TACHIE_MAX_COLUMN>* m_tachieTable;
        StoryTable<TACHIE_POSITION_MAX_COLUMN>* m_tachiePositionTable;
    };


}
#endif
