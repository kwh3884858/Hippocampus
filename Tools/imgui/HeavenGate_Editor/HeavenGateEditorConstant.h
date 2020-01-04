//
//  HeavenGateEditorConstant.hpp
//  example_osx_opengl2
//
//  Created by 威化饼干 on 24/11/2019.
//  Copyright © 2019 ImGui. All rights reserved.
//

#ifndef HeavenGateEditorConstant_h
#define HeavenGateEditorConstant_h

namespace HeavenGateEditor {

    const int     MAX_FOLDER_PATH = 265;
    const int     MAX_FILE_NAME = 64;
    const int     MAX_FOLDER_LIST = 32;
    const int     MAX_FULL_CONTENT = 6144;

    //For story content limit
    const int     MAX_NAME = 64;
    const int     MAX_CONTENT = 265;
    const int     MAX_ID = 128;

    const int     MAX_ENUM_LENGTH = 16;

    //Table
    const int     MAX_COLUMNS_CONTENT_LENGTH = 16;
    const int     FONT_SIZE_MAX_COLUMN = 2;
    const int     COLOR_MAX_COLUMN = 5;
    const int     TIP_MAX_COLUMN = 2;
    const int     PAINT_MOVE_MAX_COLUMN = 4;
    const int     CHARPTER_COLUMN = 2;
    const int     SCENE_COLUMN = 2;

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

    // Story export Folder
    const char* const STORY_EXPORT_FOLDER ="Storys_Export";


//    //Relative path form project root to story export folder
//    const char* const PATH_FROM_PROJECT_ROOT_TO_STORY_EXPORT_FOLDER =
//#ifdef _WIN32
//        "Assets\\Resources\\Storys_Export";
//#else
//        "Assets/Resources/Storys";
//#endif
//
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
        "\\TipTable.json";
#else
        "/TipsTable.json";
#endif

    //Relative path from story folder to paint move table

    const char* const PAINT_MOVE_TABLE_NAME =
#ifdef _WIN32
        "\\PaintMoveTable.json";
#else
        "/PaintMoveTable.json";
#endif

    const char* const PATH_FROM_PROJECT_ROOT_TO_FONT_FOLDER =
#ifdef _WIN32
        "Assets\\Fonts\\SourceHanSansCN-Regular.ttf";
#else
        "Assets/Fonts/SourceHanSansCN-Regular.ttf";
#endif

}
#endif /* HeavenGateEditorConstant_h */
