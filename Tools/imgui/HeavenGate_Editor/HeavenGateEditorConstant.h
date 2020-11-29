//
//  HeavenGateEditorConstant.hpp
//  example_osx_opengl2
//
//  Created by 威化饼干 on 24/11/2019.
//  Copyright © 2019 ImGui. All rights reserved.
//

#ifndef HeavenGateEditorConstant_h
#define HeavenGateEditorConstant_h

#include "StoryLayoutConstant.h"

namespace HeavenGateEditor {
    const char* const   EDITOR_VERSION = "0.0.25";
    const char* const   RELEASE_NOTE = "version 25 : Add new event type: load talk UI type";
    const int     MAX_FOLDER_PATH = 265;

    const int     MAX_FILE_NAME = 64;
    const int     MAX_FOLDER_LIST = 265;
    const int     MAX_FULL_CONTENT = 24576;

    //For story content limit
    const int     MAX_NAME = 64;
    const int     MAX_NAME_LIMIT = MAX_NAME - 4; // For UTF-8, need more space to keep string not overflow
    const int     MAX_CONTENT = 512;
    const int     MAX_CONTENT_LIMIT = MAX_CONTENT - 4;
    const int     MAX_ID_PART = 32;
    const int     MAX_EXHIBIT_NAME = 512;
    const int     MAX_EVENT_NAME = 64;

    //Table
    const int     MAX_COLUMNS_CONTENT_LENGTH = 64;
    const int     FONT_SIZE_MAX_COLUMN = MappingLayoutToArrayIndex((int)FontSizeTableLayout::Amount);
    const int     COLOR_MAX_COLUMN = MappingLayoutToArrayIndex((int)ColorTableLayout::Amount);
    const int     TIP_MAX_COLUMN = MappingLayoutToArrayIndex((int)TipTableLayout::Amount);
    const int     TACHIE_MOVE_MAX_COLUMN = MappingLayoutToArrayIndex((int)TachieMoveTableLayout::Amount);
    const int     PAUSE_MAX_COLUMN = MappingLayoutToArrayIndex((int)PauseTableLayout::Amount);
    const int     CHARACTER_COLUMN = MappingLayoutToArrayIndex((int)CharacterTableLayout::Amount);
    const int     EXHIBIT_COLUMN = MappingLayoutToArrayIndex((int)ExhibitTableLayout::Amount);
    const int     EFFECT_MAX_COLUMN = MappingLayoutToArrayIndex((int)EffectTableLayout::Amount);
    const int     BGM_MAX_COLUMN = MappingLayoutToArrayIndex((int)BgmTableLayout::Amount);
    const int     TACHIE_MAX_COLUMN = MappingLayoutToArrayIndex((int)TachieTableLayout::Amount);
    const int     TACHIE_POSITION_MAX_COLUMN = MappingLayoutToArrayIndex((int)TachiePositionTableLayout::Amount);

    //ID
    //Max ID must be (part + 1) * count, which means the total number of each part then plus the number of parts.
    const int     NUM_OF_ID_PART = 2;
    const int     MAX_ID_TITLE = MAX_ID_PART;
    const int     MAX_ID_COUNT = MAX_ID_PART;
    const int     MAX_ID = MAX_ID_TITLE + MAX_ID_COUNT + NUM_OF_ID_PART;
    enum class ID_PART
    {
        TITLE,
        COUNT
    };

    const int NUM_OF_TACHIE_COMMAND = 2;
    const int NUM_OF_TACHIE_MOVE_COMMAND = 2;
    const int MAX_TACHIE_ALIAS = MAX_COLUMNS_CONTENT_LENGTH;
    const int MAX_TACHIE_POSITION_ALIAS = MAX_COLUMNS_CONTENT_LENGTH;
    const int MAX_TACHIE = MAX_TACHIE_ALIAS + MAX_TACHIE_POSITION_ALIAS + NUM_OF_TACHIE_COMMAND;
    enum class TACHIE_COMMAND_PART{
        TACHIE_ALIAS,
        TACHIE_POSITION_ALIAS
    };

    //Tip Table
    const int     TIP_TABLE_MAX_CONTENT = 512;
    const int     EXHIBIT_TABLE_MAX_CONTENT = MAX_EXHIBIT_NAME;

    //Color
    const int    COLOR_VALUE_COLUMN = 4;
    const int    COLOR_RGBA_BASE_VALUE = 255;

    //Max number of display folders
    const int  MAX_NUM_OF_DISPLAY_FORLDERS = 50;

    //Tool folder name
    const char* const TOOL_FOLDER_NAME = "Tools";
    const char* const ASSET_FOLDER_NAME = "Assets";

    //Relative path form project root to story folder
    const char* const PATH_FROM_PROJECT_ROOT_TO_STORY_FOLDER =
#ifdef _WIN32
        "Assets\\Resources\\Storys";
#else
        "Assets/Resources/Storys";
#endif

    const char* const PATH_FROM_PROJECT_ROOT_TO_AUTOSAVE_FOLDER =
#ifdef _WIN32
        "Assets\\Resources\\Storys\\AutoSave~";
#else
        "Assets/Resources/Storys/AutoSave~";
#endif

    // Story export Folder
    const char* const STORY_EXPORT_FOLDER ="Storys_Export";

    // Story auto save folder
    const char* const AUTO_SAVE_FOLDER = "AutoSave~";

    // Story default file name
    const char* const DEFAULT_FILE_NAME = "Untitled";

// Delimiter
const char* const DELIMITER =
#ifdef _WIN32
        "\\";
#else
        "/";
#endif

//    //Relative path form project root to story export folder
//    const char* const PATH_FROM_PROJECT_ROOT_TO_STORY_EXPORT_FOLDER =
//#ifdef _WIN32
//        "Assets\\Resources\\Storys_Export";
//#else
//        "Assets/Resources/Storys";
//#endif
//

    //Suffix of tables
const char* const TableSuffix = "Table";

    //Relative path from story folder to font size table

    const char* const FONT_TABLE_NAME = 
#ifdef _WIN32
    "\\FontSizeTable.json";
#else
    "/FontSizeTable.json";
#endif


    //Relative path from story folder to color table

    const char* const COLOR_TABLE_NAME =
#ifdef _WIN32
        "\\ColorTable.json";
#else
        "/ColorTable.json";
#endif

    //Relative path from story folder to tip table

    const char* const TIP_TABLE_NAME =
#ifdef _WIN32
        "\\TipsTable.json";
#else
        "/TipsTable.json";
#endif

    //Relative path from story folder to paint move table

    const char* const PAINT_MOVE_TABLE_NAME =
#ifdef _WIN32
        "\\TachieMoveTable.json";
#else
        "/TachieMoveTable.json";
#endif

    //Relative path from story folder to chapter table

    const char* const CHAPTER_TABLE_NAME =
#ifdef _WIN32
        "\\ChapterTable.json";
#else
        "/ChapterTable.json";
#endif

    //Relative path from story folder to scene table

    const char* const SCENE_TABLE_NAME =
#ifdef _WIN32
        "\\SceneTable.json";
#else
        "/SceneTable.json";
#endif

    //Relative path from story folder to character table

    const char* const CHARACTER_TABLE_NAME =
#ifdef _WIN32
        "\\CharacterTable.json";
#else
        "/CharacterTable.json";
#endif

    //Relative path from story folder to pause table

    const char* const PAUSE_TABLE_NAME =
#ifdef _WIN32
        "\\PauseTable.json";
#else
        "/PauseTable.json";
#endif

    //Relative path from story folder to exhibit table

    const char* const EXHIBIT_TABLE_NAME =
#ifdef _WIN32
        "\\ExhibitTable.json";
#else
        "/ExhibitTable.json";
#endif

    //Relative path from story folder to effect table

    const char* const EFFECT_TABLE_NAME =
#ifdef _WIN32
        "\\EffectTable.json";
#else
        "/EffectTable.json";
#endif

    //Relative path from story folder to bgm table

    const char* const BGM_TABLE_NAME =
#ifdef _WIN32
        "\\BgmTable.json";
#else
        "/BgmTable.json";
#endif

    //Relative path from story folder to tachie table

    const char* const TACHIE_TABLE_NAME =
#ifdef _WIN32
        "\\TachieTable.json";
#else
        "/TachieTable.json";
#endif

    //Relative path from story folder to tachie position table

    const char* const TACHIE_POSITION_TABLE_NAME =
#ifdef _WIN32
        "\\TachiePositionTable.json";
#else
        "/TachiePositionTable.json";
#endif

    const char* const PATH_FROM_PROJECT_ROOT_TO_FONT_FOLDER =
#ifdef _WIN32
        "Assets\\data\\graphics\\Fonts\\Fonts_SourceHanSansCN-Regular.ttf";
#else
        "Assets/data/graphics/Fonts/Fonts_SourceHanSansCN-Regular.ttf";
#endif

    // Artist Tool
    // Character Tachie
#ifdef _WIN32
    const char*  const PATH_FROM_PROJECT_ROOT_TO_TACHIE = "Assets\\data\\graphics\\UI\\Characters";
#endif

    // Interactable CG
#ifdef _WIN32
    const char*  const PATH_FROM_PROJECT_ROOT_TO_INTERACTABLE_CG = "Assets\\data\\graphics\\UI\\CG\\Scene";
#endif

    // Talk Panel Background
#ifdef _WIN32
    const char*  const PATH_FROM_PROJECT_ROOT_TO_TALK_BACKGROUND = "Assets\\data\\graphics\\UI\\CG\\TalkBackground";
#endif

    //BGM
#ifdef _WIN32
    const char*  const PATH_FROM_PROJECT_ROOT_TO_BGM = "Assets\\Resources\\Sound\\Bgm";
#endif

    //Effect
#ifdef _WIN32
    const char*  const PATH_FROM_PROJECT_ROOT_TO_EFFECT = "Assets\\Resources\\Sound\\Effect";
#endif

//Alias
    using UniqueID = unsigned long int;

    //Error Message
    const char* const Error_Message_Label_Jump_Death_Lock = "Label and jump node will cause endless loop.";
    const char* const Error_Message_Content_Over_Limit = "Node content is too much to over max line limit.";

}
#endif /* HeavenGateEditorConstant_h */
