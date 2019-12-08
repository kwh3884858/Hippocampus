//
//  StoryTable.hpp
//  example_osx_opengl2
//
//  Created by 威化饼干 on 7/12/2019.
//  Copyright © 2019 ImGui. All rights reserved.
//

#ifndef StoryTable_h
#define StoryTable_h

#include <stdio.h>
#include "nlohmann/json.hpp"
#include "HeavenGateEditorConstant.h"



namespace HeavenGateEditor {
    using json = nlohmann::json;

template<int row, int column>
class StoryTable{
public:
    StoryTable(int row,int column);


private:
    int m_row;
    int m_column;
    char (*m_columnName)[MAX_COLUMNS_CONTENT_LENGTH];
    char (*m_content)[MAX_COLUMNS_CONTENT_LENGTH];
};

}

#endif /* StoryTable_h */
