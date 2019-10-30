//
//  HeavenGateEditorWindow.cpp
//  example_osx_opengl2
//
//  Created by 威化饼干 on 29/10/2019.
//  Copyright © 2019 ImGui. All rights reserved.
//
#include "imgui.h"

#include "HeavenGateEditorWindow.h"
namespace HeavenGateEditor {

void ShowEditorWindow(bool* isOpenPoint){


    // Demonstrate the various window flags. Typically you would just use the default!
    static bool no_titlebar = false;
    static bool no_scrollbar = false;
    static bool no_menu = false;
    static bool no_move = false;
    static bool no_resize = false;
    static bool no_collapse = false;
    static bool no_close = false;
    static bool no_nav = false;
    static bool no_background = false;
    static bool no_bring_to_front = false;

    ImGuiWindowFlags window_flags = 0;
     if (no_titlebar)        window_flags |= ImGuiWindowFlags_NoTitleBar;
      if (no_scrollbar)       window_flags |= ImGuiWindowFlags_NoScrollbar;
      if (!no_menu)           window_flags |= ImGuiWindowFlags_MenuBar;
      if (no_move)            window_flags |= ImGuiWindowFlags_NoMove;
      if (no_resize)          window_flags |= ImGuiWindowFlags_NoResize;
      if (no_collapse)        window_flags |= ImGuiWindowFlags_NoCollapse;
      if (no_nav)             window_flags |= ImGuiWindowFlags_NoNav;
      if (no_background)      window_flags |= ImGuiWindowFlags_NoBackground;
      if (no_bring_to_front)  window_flags |= ImGuiWindowFlags_NoBringToFrontOnFocus;
      if (no_close)           isOpenPoint = NULL; // Don't pass our bool* to Begin


    // We specify a default position/size in case there's no data in the .ini file. Typically this isn't required! We only do it to make the Demo applications a little more welcoming.
    ImGui::SetNextWindowPos(ImVec2(650, 20), ImGuiCond_FirstUseEver);
    ImGui::SetNextWindowSize(ImVec2(550, 680), ImGuiCond_FirstUseEver);

    // Main body of the Demo window starts here.
    if (!ImGui::Begin("Dear ImGui Demo", isOpenPoint, window_flags))
    {


        // Early out if the window is collapsed, as an optimization.
        ImGui::End();
        return;
    }

    // End of ShowDemoWindow()
    ImGui::End();
}
}

