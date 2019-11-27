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

#define     MAX_FOLDER_PATH         265
#define     MAX_FILE_NAME           64
#define     MAX_FOLDER_LIST         32
#define     MAX_FULL_CONTENT        512

//For story content limit
#define     MAX_NAME                64
#define     MAX_CONTENT             265

//Max number of display folders
static const int  MAX_NUM_OF_DISPLAY_FORLDERS = 50;

//Tool folder name
static const char* const TOOL_FOLDER_NAME= "Tools";

//Relative path form project root to story folder
static const char* const PATH_FROM_PROJECT_ROOT_TO_STORY_FOLDER =
#ifdef _WIN32
"Assets\\Storys";
#else
"Assets/Storys";
#endif

}
#endif /* HeavenGateEditorConstant_h */
