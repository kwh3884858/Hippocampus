//
//  StoryTable.cpp
//  example_osx_opengl2
//
//  Created by 威化饼干 on 7/12/2019.
//  Copyright © 2019 ImGui. All rights reserved.
//

#include "StoryTable.h"

namespace HeavenGateEditor {
    template<int column>
    StoryRow<column>::StoryRow()
    {
        m_content = new char[column][MAX_COLUMNS_CONTENT_LENGTH];

        memset(m_content, 0, sizeof(m_content));
        m_size = 0;
    }

    template<int column>
    StoryRow<column>::~StoryRow()
    {
        if (m_content != nullptr)
        {
            delete m_content;
        }
        m_content = nullptr;

    }

    template<int column>
    int StoryRow<column>::Size()const
    {
        return m_size;
    }

    template<int column>
    bool StoryRow<column>::IsFull() const
    {
        return m_size >= column;
    }

    template<int column>
    bool StoryRow<column>::Push(const char * content)
    {
        return Set(m_size, content);
    }

    template<int column>
    bool StoryRow<column>::Set(int index, const char * content)
    {
        if (index >= column || index < 0)
        {
            return false;
        }

        strcpy(m_content[index], content);
        return true;
    }

    template<int column>
    const char * StoryRow<column>::Get(int index)const
    {
        if (index >= column || index < 0)
        {
            return nullptr;
        }
        return m_content[index];
    }


    /*   template<int column>
        StoryTable<column>::StoryTable()
       {
           m_name = new StoryRow<column>;
           m_rowSize = 0;
       }
   */

    template<int column>
    StoryTable<column>::StoryTable()
    {
        //m_name = new StoryRow<column>;
        m_rowSize = 0;
    }

    template<int column>
    StoryTable<column>::~StoryTable()
    {
        if (m_name != nullptr)
        {
            delete m_name;
        }
        m_name != nullptr;

        for (int i = 0; i < m_content.size(); i++)
        {
            if (m_content[i] != nullptr)
            {
                delete m_content[i];
            }

            m_content[i] = nullptr;
        }
        m_content.clear();

    }

    template<int column>
    bool StoryTable<column>::PushName(const char * name)
    {
        return m_name->Push(name);
    }

    template<int column>
    bool StoryTable<column>::SetName(int index, const char * name)
    {
        return m_name->Set(index, name);
    }

    template<int column>
    bool StoryTable<column>::PushContent(const char * content)
    {
        StoryRow<column>* aRow = m_content.back();

        if (aRow->IsFull())
        {
            if (m_rowSize + 1 >= row) {
                return  false;
            }
            aRow = new StoryRow<column>;
            m_content.push_back(row);
            m_rowSize++;
        }
        aRow.Push(content);

        return true;
    }

    template<int column>
    bool StoryTable<column>::SetContent(int row, int index, const char * content)
    {

        if (row < 0 || row >= m_rowSize)
        {
            return false;
        }

        StoryRow<column>* aRow = m_content[row];
        if (index >= aRow->Size() || index < 0)
        {
            return false;
        }
        aRow->Set(index, content);

        return true;
    }

    template<int column>
    const char * StoryTable<column>::GetName(int index)
    {
        return m_name->Get(index);
    }

    template<int column>
    const char * StoryTable<column>::GetContent(int row, int index)
    {
        if (row < 0 || row >= m_rowSize)
        {
            return false;
        }

        StoryRow<column>* aRow = m_content[row];

        if (index >= aRow->Size() || index < 0)
        {
            return nullptr;
        }
        return aRow->Get(index);


    }



    template<int column>
    void to_json(json& j, const StoryTable<column>& p) {
        for (int i = 0; i < j.size(); i++)
        {

        }
    }

    template<int column>
    void to_json(json& j, const StoryRow<column>& p) {

    }

    template<int column>
    void from_json(const json& j, StoryTable<column>& p) {

    }

    template<int column>
    void from_json(const json& j, StoryRow<column>& p) {

    }
}
