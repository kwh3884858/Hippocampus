
#pragma once

#ifndef StoryRow_h
#define StoryRow_h

#include "nlohmann/json.hpp"

#include "HeavenGateEditorConstant.h"
#include "StoryLayoutConstant.h"

namespace HeavenGateEditor {
using json = nlohmann::json;

template<int column, int MAX_CONTENT_LENGTH = MAX_COLUMNS_CONTENT_LENGTH>
class StoryRow
{
public:
    StoryRow();
    StoryRow(const StoryRow<column, MAX_CONTENT_LENGTH>& storyRow);
    StoryRow(StoryRow<column, MAX_CONTENT_LENGTH>&& storyRow);
    ~StoryRow();

    inline int Size()const;
    inline bool IsFull()const;
    bool Push(const char* content);
    bool Set(int index, const char* content);
    const char* Get(int index)const;
private:
    int m_size;
    char(*m_content)[MAX_CONTENT_LENGTH];
};


template<int column, int MAX_CONTENT_LENGTH = MAX_COLUMNS_CONTENT_LENGTH>
void to_json(json& j, const StoryRow<column, MAX_CONTENT_LENGTH>& p);

template<int column, int MAX_CONTENT_LENGTH = MAX_COLUMNS_CONTENT_LENGTH>
void from_json(const json& j, StoryRow<column, MAX_CONTENT_LENGTH>& p);


template<int column, int MAX_CONTENT_LENGTH>
void to_json(json& j, const StoryRow<column, MAX_CONTENT_LENGTH>& p)
{
    j = json{
        {fontSizeTableString[(int)FontSizeTableLayout::Alias],          p.Get(0) },
        {fontSizeTableString[(int)FontSizeTableLayout::Size],           p.Get(1) }
    };
}
template<int column, int MAX_CONTENT_LENGTH >
void from_json(const json& j, StoryRow<column, MAX_CONTENT_LENGTH>& p)
{

}

template<int column, int MAX_CONTENT_LENGTH>
StoryRow<column, MAX_CONTENT_LENGTH>::StoryRow()
{
    m_content = new char[column][MAX_CONTENT_LENGTH];

    memset(m_content, 0, MAX_CONTENT_LENGTH * column);
    m_size = 0;

}

template<int column, int MAX_CONTENT_LENGTH>
StoryRow<column, MAX_CONTENT_LENGTH>::StoryRow(const StoryRow<column, MAX_CONTENT_LENGTH>& storyRow)
{
    m_size = storyRow.m_size;

    m_content = new char[column][MAX_CONTENT_LENGTH];
    memset(m_content, storyRow.m_content, MAX_CONTENT_LENGTH * column);
}


template<int column, int MAX_CONTENT_LENGTH>
HeavenGateEditor::StoryRow<column, MAX_CONTENT_LENGTH>::StoryRow(StoryRow<column, MAX_CONTENT_LENGTH>&& storyRow)
{
    m_size = storyRow.m_size;

    m_content = storyRow.m_content;
    storyRow.m_content = nullptr;
}

template<int column, int MAX_CONTENT_LENGTH>
StoryRow<column, MAX_CONTENT_LENGTH>::~StoryRow()
{
    if (m_content != nullptr)
    {
        delete m_content;
    }
    m_content = nullptr;

}

template<int column, int MAX_CONTENT_LENGTH>
int StoryRow<column, MAX_CONTENT_LENGTH>::Size()const
{
    return m_size;
}

template<int column, int MAX_CONTENT_LENGTH>
bool StoryRow<column, MAX_CONTENT_LENGTH>::IsFull() const
{
    return m_size >= column;
}

template<int column, int MAX_CONTENT_LENGTH>
bool StoryRow<column, MAX_CONTENT_LENGTH>::Push(const char* content)
{
    return Set(m_size++, content);
}

template<int column, int MAX_CONTENT_LENGTH>
bool StoryRow<column, MAX_CONTENT_LENGTH>::Set(int index, const char* content)
{
    if (index >= column || index < 0)
    {
        return false;
    }

    strcpy(m_content[index], content);
    return true;
}

template<int column, int MAX_CONTENT_LENGTH>
const char* StoryRow<column, MAX_CONTENT_LENGTH>::Get(int index)const
{
    if (index >= column || index < 0)
    {
        return nullptr;
    }
    return m_content[index];
}


}

#endif /* StoryRow_h*/
