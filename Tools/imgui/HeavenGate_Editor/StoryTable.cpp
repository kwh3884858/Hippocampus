//
//  StoryTable.cpp
//  example_osx_opengl2
//
//  Created by 威化饼干 on 7/12/2019.
//  Copyright © 2019 ImGui. All rights reserved.
//

#include "StoryTable.h"

namespace HeavenGateEditor {


StoryTable::StoryTable(int row,int column){
    if (row <= 0) {
        row = 1;
    }

    if (column <= 0) {
        column = 1;
    }
    m_columnName = new char[row][column][MAX_COLUMNS_CONTENT_LENGTH];


}


}
