//
//  StoryTable.hpp
//  example_osx_opengl2
//
//  Created by 威化饼干 on 7/12/2019.
//  Copyright © 2019 ImGui. All rights reserved.
//
#pragma once

#ifndef StoryTable_h
#define StoryTable_h

#include <stdio.h>
#include <vector>

#include "nlohmann/json.hpp"
#include "HeavenGateEditorConstant.h"


namespace HeavenGateEditor {
    using json = nlohmann::json;
    using std::vector;

    //enum class TableLayout :int;
    //enum class FontSizeTableLayout :int;

    enum class TableLayout {

        Type = 0,
        Value = 1

    };

    enum class FontSizeTableLayout {
        Type = 0,
        Alias = 1,
        Size = 2
    };

    enum class ColorTableLayout {
        Type = 0,
        Alias = 1,
        //Size = 2
        R,
        G,
        B,
        A
    };

    enum class TipTableLayout {
        Type = 0,
        Tip = 1,
        Description = 2
    };

    enum class PaintMoveTableLayout {
        Type = 0,
        MoveAlias = 1,
        StartPoint = 2,
        EndPoint = 3,
        MoveType = 4
    };

    enum class ChapterTableLayout {
        Type = 0,
        Chapter = 1,
        Description = 2
    };

    enum class SceneTableLayout {
        Type = 0,
        Scene = 1,
        Description = 2
    };

    enum class CharacterTableLayout {
        Type = 0,
        Character = 1,
        Description = 2
    };

    enum class PauseTableLayout {
        Type = 0,
        Pause = 1,
        Time = 2
    };

    enum class ExhibitTableLayout {
        Type = 0,
        Exhibit = 1,
        Description = 2
    };

    const char tableString[][MAX_ENUM_LENGTH] = {
        "type",
        "value"
    };

    const char fontSizeTableString[][MAX_ENUM_LENGTH] = {
        "fontSize",
        "alias",
        "size"
    };

    const char colorTableString[][MAX_ENUM_LENGTH] = {
        "color",
        "alias",
        //"size"
        "r",
        "g",
        "b",
        "a"
    };

    const char tipTableString[][MAX_ENUM_LENGTH] = {
        "tip",
        "tip",
        "description"
    };

    const char paintMoveTableString[][MAX_ENUM_LENGTH] = {
        "paintMove",
        "moveAlias",
        "startPoint",
        "endPoint",
        "moveType",

    };

    const char chapterTableString[][MAX_ENUM_LENGTH] = {
        "chapter",
        "chapter",
        "description"

    };

    const char sceneTableString[][MAX_ENUM_LENGTH] = {
        "scene",
        "scene",
        "description"

    };

    const char characterTableString[][MAX_ENUM_LENGTH] = {
        "character",
        "character",
        "description"

    };

    const char pauseTableString[][MAX_ENUM_LENGTH] = {
        "pause",
        "pause",
        "time"

    };

    const char exhibitTableString[][MAX_ENUM_LENGTH] = {
        "exhibit",
        "exhibit",
        "description"

    };

    template<int column >
    class StoryRow
    {
    public:
        StoryRow();
        StoryRow(const StoryRow& storyRow);
        StoryRow(StoryRow&& storyRow);
        ~StoryRow();

        inline int Size()const;
        inline bool IsFull()const;
        bool Push(const char* content);
        bool Set(int index, const char* content);
        const char* Get(int index)const;
    private:
        int m_size;
        char(*m_content)[MAX_COLUMNS_CONTENT_LENGTH];
    };

    enum class TableType
    {
        None = 0,
        Font_Size,
        Color,
        Tips,
        Paint_Move,
        Chapter,
        Scene,
        Character,
        Pause,
        Exhibit
    };

    template<int column >
    class StoryTable {
    public:



    public:
        StoryTable();
        StoryTable(const StoryTable& storyTable);
        StoryTable(StoryTable&& storyTable);
        ~StoryTable();

        StoryTable& operator=(StoryTable&& storyTable)noexcept;

        bool PushName(const char* name);
        bool SetName(int index, const char* name);
        bool PushContent(const char* content);
        bool SetContent(int rowIndex, int index, const char* content);
        inline const char* GetName(int index);
        inline const char* GetContent(int rowIndex, int index)const;
        char* GetContent(int rowIndex, int index);

        const StoryRow<column>* const GetRow(int index)const;
        StoryRow<column>* AddRow();
        StoryRow<column>* RemoveRow();
        void PushRow(StoryRow<column>* storyRow);

        int GetSize()const;
        void SetTableType(TableType tableType);
        TableType GetTableType()const;
    private:
        void Clear();

        StoryRow<column>* m_name;
        vector<StoryRow<column>*> m_content;
        TableType m_tableType;

    };



    template<int column>
    void to_json(json& j, const StoryTable< column>& p);

    template<int column>
    void to_json(json& j, const StoryRow<column>& p);

    template< int column>
    void from_json(const json& j, StoryTable< column>& p);

    template<int column>
    void from_json(const json& j, StoryRow<column>& p);




    template<int column>
    StoryRow<column>::StoryRow()
    {
        m_content = new char[column][MAX_COLUMNS_CONTENT_LENGTH];

        memset(m_content, 0, MAX_COLUMNS_CONTENT_LENGTH * column);
        m_size = 0;

    }

    template<int column >
    HeavenGateEditor::StoryRow<column>::StoryRow(const StoryRow& storyRow)
    {
        m_size = storyRow.m_size;

        m_content = new char[column][MAX_COLUMNS_CONTENT_LENGTH];
        memset(m_content, storyRow.m_content, MAX_COLUMNS_CONTENT_LENGTH * column);
    }


    template<int column >
    HeavenGateEditor::StoryRow<column>::StoryRow(StoryRow&& storyRow)
    {
        m_size = storyRow.m_size;

        m_content = storyRow.m_content;
        storyRow.m_content = nullptr;
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
    bool StoryRow<column>::Push(const char* content)
    {
        return Set(m_size++, content);
    }

    template<int column>
    bool StoryRow<column>::Set(int index, const char* content)
    {
        if (index >= column || index < 0)
        {
            return false;
        }

        strcpy(m_content[index], content);
        return true;
    }

    template<int column>
    const char* StoryRow<column>::Get(int index)const
    {
        if (index >= column || index < 0)
        {
            return nullptr;
        }
        return m_content[index];
    }

    //=======================Story Table========================

    template<int column>
    StoryTable<column>::StoryTable()
    {
        m_name = new StoryRow<column>;
        m_tableType = TableType::None;
    }
    template<int column >
    HeavenGateEditor::StoryTable<column>::StoryTable(const StoryTable& storyTable)
    {

        m_name(storyTable.m_name);
        for (int i = 0; i < m_content.size(); i++) {
            StoryRow<column>* tmpRow = new StoryRow<column>(*(storyTable.m_content[i]));
            m_content.push_back(tmpRow);
        }

        m_tableType = storyTable.m_tableType;
    }

    template<int column >
    HeavenGateEditor::StoryTable<column>::StoryTable(StoryTable&& storyTable) :
        m_tableType(storyTable.m_tableType)
    {
        m_name = storyTable.m_name;
        m_content = storyTable.m_content;

        storyTable.m_name = nullptr;
        storyTable.m_content.clear();
    }
    template<int column>
    StoryTable<column>::~StoryTable()
    {
        Clear();
    }

    template<int column >
    void StoryTable<column>::Clear()
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

    template<int column >
    StoryTable<column>& StoryTable<column>::operator=(StoryTable&& storyTable) noexcept
    {
        if (this == &storyTable) {
            return *this;
        }

        Clear();
        m_name = storyTable.m_name;
        m_content = storyTable.m_content;
        m_tableType = storyTable.m_tableType;

        storyTable.m_name = nullptr;
        storyTable.m_content.clear();

        return *this;
    }

    template<int column>
    bool StoryTable<column>::PushName(const char* name)
    {
        return m_name->Push(name);
    }

    template<int column>
    bool StoryTable<column>::SetName(int index, const char* name)
    {
        return m_name->Set(index, name);
    }

    template<int column>
    bool StoryTable<column>::PushContent(const char* content)
    {
        StoryRow<column>* aRow;

        if (m_content.empty())
        {
            aRow = AddRow();
        }
        else
        {
            aRow = m_content.back();
        }


        if (aRow->IsFull())
        {
            aRow = AddRow();
        }
        aRow->Push(content);

        return true;
    }

    template<int column>
    bool StoryTable<column>::SetContent(int row, int index, const char* content)
    {

        if (row < 0 || row >= m_content.size())
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
    const char* StoryTable<column>::GetName(int index)
    {
        return m_name->Get(index);
    }

    template<int column>
    const char* StoryTable<column>::GetContent(int row, int index)const
    {
        if (row < 0 || row >= m_content.size())
        {
            return nullptr;
        }

        StoryRow<column>* aRow = m_content[row];

        //        if (index >= aRow->Size() || index < 0)
        //        {
        //            return nullptr;
        //        }
        return aRow->Get(index);


    }

    template<int column >
    char* StoryTable<column>::GetContent(int rowIndex, int index) {
        return const_cast<char*>(const_cast<const StoryTable<column>*>(this)->GetContent(rowIndex, index));
    }


    template<int column >
    const StoryRow<column>* const HeavenGateEditor::StoryTable<column>::GetRow(int index) const
    {
        return m_content[index];
    }

    template<int column >
    int HeavenGateEditor::StoryTable<column>::GetSize() const
    {
        return m_content.size();
    }

    template<int column >
    StoryRow<column>* HeavenGateEditor::StoryTable<column>::AddRow()
    {
        StoryRow<column>* newRow = new StoryRow<column>;
        PushRow(newRow);
        return newRow;
    }

    template<int column >
    HeavenGateEditor::StoryRow<column>* HeavenGateEditor::StoryTable<column>::RemoveRow()
    {
        StoryRow<column>* row = m_content.back();
        m_content.pop_back();
        return row;
    }

    template<int column >
    void HeavenGateEditor::StoryTable<column>::PushRow(StoryRow<column>* storyRow)
    {
        m_content.push_back(storyRow);
    }


    template<int column >
    void HeavenGateEditor::StoryTable<column>::SetTableType(TableType tableType)
    {
        m_tableType = tableType;
    }

    template<int column>
    TableType StoryTable<column>::GetTableType() const
    {
        return m_tableType;
    }


    //========================Json========================== 

    template<int column>
    void to_json(json& j, const StoryTable<column>& p) {

        const StoryRow<column>* tmp;
        switch (p.GetTableType())
        {

        case TableType::Font_Size:
        {
            j[tableString[(int)TableLayout::Type]] = fontSizeTableString[(int)FontSizeTableLayout::Type];
            if (p.GetSize() == 0)
            {
                j[tableString[(int)TableLayout::Value]] = json::array();
            }
            for (int i = 0; i < p.GetSize(); i++)
            {
                tmp = p.GetRow(i);
                j[tableString[(int)TableLayout::Value]].push_back(json{
    {fontSizeTableString[(int)FontSizeTableLayout::Alias],          tmp->Get(0) },
    {fontSizeTableString[(int)FontSizeTableLayout::Size],           tmp->Get(1) }
                    });
            }
            break;
        }

        case TableType::Color:
        {
            j[tableString[(int)TableLayout::Type]] = colorTableString[(int)ColorTableLayout::Type];
            if (p.GetSize() == 0)
            {
                j[tableString[(int)TableLayout::Value]] = json::array();
            }
            for (int i = 0; i < p.GetSize(); i++)
            {
                tmp = p.GetRow(i);
                j[tableString[(int)TableLayout::Value]].push_back(json{
    {colorTableString[(int)ColorTableLayout::Alias],          tmp->Get(0) },
    {colorTableString[(int)ColorTableLayout::R],           tmp->Get(1) },
    {colorTableString[(int)ColorTableLayout::G],           tmp->Get(2) },
    {colorTableString[(int)ColorTableLayout::B],           tmp->Get(3) },
    {colorTableString[(int)ColorTableLayout::A],           tmp->Get(4) }
                    });
            }
            break;
        }

        case TableType::Tips:
        {
            j[tableString[(int)TableLayout::Type]] = tipTableString[(int)TipTableLayout::Type];
            if (p.GetSize() == 0)
            {
                j[tableString[(int)TableLayout::Value]] = json::array();
            }
            for (int i = 0; i < p.GetSize(); i++)
            {
                tmp = p.GetRow(i);
                j[tableString[(int)TableLayout::Value]].push_back(json{
    {tipTableString[(int)TipTableLayout::Tip],        tmp->Get(0) },
    {tipTableString[(int)TipTableLayout::Description],           tmp->Get(1) }
                    });
            }
            break;
        }

        case TableType::Paint_Move:
        {
            j[tableString[(int)TableLayout::Type]] = paintMoveTableString[(int)PaintMoveTableLayout::Type];
            if (p.GetSize() == 0)
            {
                j[tableString[(int)TableLayout::Value]] = json::array();
            }
            for (int i = 0; i < p.GetSize(); i++)
            {
                tmp = p.GetRow(i);
                j[tableString[(int)TableLayout::Value]].push_back(json{
    {paintMoveTableString[(int)PaintMoveTableLayout::MoveAlias],          tmp->Get(0) },
    {paintMoveTableString[(int)PaintMoveTableLayout::StartPoint],          tmp->Get(1) },
    {paintMoveTableString[(int)PaintMoveTableLayout::EndPoint],          tmp->Get(2) },
    {paintMoveTableString[(int)PaintMoveTableLayout::MoveType],           tmp->Get(3) }
                    });
            }
            break;
        }

        case TableType::Chapter:
        {
            j[tableString[(int)TableLayout::Type]] = chapterTableString[(int)ChapterTableLayout::Type];
            if (p.GetSize() == 0)
            {
                j[tableString[(int)TableLayout::Value]] = json::array();
            }
            for (int i = 0; i < p.GetSize(); i++)
            {
                tmp = p.GetRow(i);
                j[tableString[(int)TableLayout::Value]].push_back(json{
    {chapterTableString[(int)ChapterTableLayout::Chapter],          tmp->Get(0) },
    {chapterTableString[(int)ChapterTableLayout::Description],          tmp->Get(1) }
                    });
            }
            break;
        }

        case TableType::Scene:
        {
            j[tableString[(int)TableLayout::Type]] = sceneTableString[(int)SceneTableLayout::Type];
            if (p.GetSize() == 0)
            {
                j[tableString[(int)TableLayout::Value]] = json::array();
            }
            for (int i = 0; i < p.GetSize(); i++)
            {
                tmp = p.GetRow(i);
                j[tableString[(int)TableLayout::Value]].push_back(json{
    {sceneTableString[(int)SceneTableLayout::Scene],          tmp->Get(0) },
    {sceneTableString[(int)SceneTableLayout::Description],          tmp->Get(1) }
                    });
            }
            break;
        }

        case TableType::Character:
        {
            j[tableString[(int)TableLayout::Type]] = characterTableString[(int)CharacterTableLayout::Type];
            if (p.GetSize() == 0)
            {
                j[tableString[(int)TableLayout::Value]] = json::array();
            }
            for (int i = 0; i < p.GetSize(); i++)
            {
                tmp = p.GetRow(i);
                j[tableString[(int)TableLayout::Value]].push_back(json{
    {characterTableString[(int)CharacterTableLayout::Character],          tmp->Get(0) },
    {characterTableString[(int)CharacterTableLayout::Description],          tmp->Get(1) }
                    });
            }
            break;
        }

        case TableType::Pause:
        {
            j[tableString[(int)TableLayout::Type]] = pauseTableString[(int)PauseTableLayout::Type];
            if (p.GetSize() == 0)
            {
                j[tableString[(int)TableLayout::Value]] = json::array();
            }
            for (int i = 0; i < p.GetSize(); i++)
            {
                tmp = p.GetRow(i);
                j[tableString[(int)TableLayout::Value]].push_back(json{
    {pauseTableString[(int)PauseTableLayout::Pause],          tmp->Get(0) },
    {pauseTableString[(int)PauseTableLayout::Time],          tmp->Get(1) }
                    });
            }
            break;
        }

        case TableType::Exhibit:
        {
            j[tableString[(int)TableLayout::Type]] = exhibitTableString[(int)ExhibitTableLayout::Type];
            if (p.GetSize() == 0)
            {
                j[tableString[(int)TableLayout::Value]] = json::array();
            }
            for (int i = 0; i < p.GetSize(); i++)
            {
                tmp = p.GetRow(i);
                j[tableString[(int)TableLayout::Value]].push_back(json{
    {exhibitTableString[(int)ExhibitTableLayout::Exhibit],          tmp->Get(0) },
    {exhibitTableString[(int)ExhibitTableLayout::Description],          tmp->Get(1) }
                    });
            }
            break;
        }

        default:

            return;
        }

        //if (p.GetSize() == 0)
        //{
        //    j[tableString[(int)TableLayout::Value]] = json::array();
        //}

        //for (int i = 0; i < p.GetSize(); i++)
        //{
        //    j[tableString[(int)TableLayout::Value]].push_back(*(p.GetRow(i)));

        //}
    }

    template<int column>
    void to_json(json& j, const StoryRow<column>& p)
    {
        j = json{
            {fontSizeTableString[(int)FontSizeTableLayout::Alias],          p.Get(0) },
            {fontSizeTableString[(int)FontSizeTableLayout::Size],           p.Get(1) }
        };
    }

    template<int column>
    void from_json(const json& j, StoryTable<column>& p) {

        char typeString[MAX_ENUM_LENGTH];
        strcpy(typeString, j.at(tableString[(int)TableLayout::Type]).get_ptr<const json::string_t*>()->c_str());

        json values = j.at(tableString[(int)TableLayout::Value]);
        /*get_ptr<const json::string_t *>()->c_str();*/

        if (strcmp(typeString, fontSizeTableString[(int)FontSizeTableLayout::Type]) == 0) {
            p.SetTableType(TableType::Font_Size);
            char content[MAX_COLUMNS_CONTENT_LENGTH];
            for (int i = 0; i < values.size(); i++)
            {

                strcpy(content, values[i].at(fontSizeTableString[(int)FontSizeTableLayout::Alias]).get_ptr<const json::string_t*>()->c_str());
                p.PushContent(content);
                strcpy(content, values[i].at(fontSizeTableString[(int)FontSizeTableLayout::Size]).get_ptr<const json::string_t*>()->c_str());
                p.PushContent(content);

            }

            return;
        }

        if (strcmp(typeString, colorTableString[(int)ColorTableLayout::Type]) == 0) {
            p.SetTableType(TableType::Color);
            char content[MAX_COLUMNS_CONTENT_LENGTH];
            for (int i = 0; i < values.size(); i++)
            {

                strcpy(content, values[i].at(colorTableString[(int)ColorTableLayout::Alias]).get_ptr<const json::string_t*>()->c_str());
                p.PushContent(content);
                strcpy(content, values[i].at(colorTableString[(int)ColorTableLayout::R]).get_ptr<const json::string_t*>()->c_str());
                p.PushContent(content);
                strcpy(content, values[i].at(colorTableString[(int)ColorTableLayout::G]).get_ptr<const json::string_t*>()->c_str());
                p.PushContent(content);
                strcpy(content, values[i].at(colorTableString[(int)ColorTableLayout::B]).get_ptr<const json::string_t*>()->c_str());
                p.PushContent(content);
                strcpy(content, values[i].at(colorTableString[(int)ColorTableLayout::A]).get_ptr<const json::string_t*>()->c_str());
                p.PushContent(content);
            }

            return;
        }

        if (strcmp(typeString, tipTableString[(int)TipTableLayout::Type]) == 0) {
            p.SetTableType(TableType::Tips);
            char content[MAX_COLUMNS_CONTENT_LENGTH];
            for (int i = 0; i < values.size(); i++)
            {

                strcpy(content, values[i].at(tipTableString[(int)TipTableLayout::Tip]).get_ptr<const json::string_t*>()->c_str());
                p.PushContent(content);
                strcpy(content, values[i].at(tipTableString[(int)TipTableLayout::Description]).get_ptr<const json::string_t*>()->c_str());
                p.PushContent(content);

            }

            return;
        }

        if (strcmp(typeString, paintMoveTableString[(int)PaintMoveTableLayout::Type]) == 0) {
            p.SetTableType(TableType::Paint_Move);
            char content[MAX_COLUMNS_CONTENT_LENGTH];
            for (int i = 0; i < values.size(); i++)
            {

                strcpy(content, values[i].at(paintMoveTableString[(int)PaintMoveTableLayout::MoveAlias]).get_ptr<const json::string_t*>()->c_str());
                p.PushContent(content);
                strcpy(content, values[i].at(paintMoveTableString[(int)PaintMoveTableLayout::StartPoint]).get_ptr<const json::string_t*>()->c_str());
                p.PushContent(content);
                strcpy(content, values[i].at(paintMoveTableString[(int)PaintMoveTableLayout::EndPoint]).get_ptr<const json::string_t*>()->c_str());
                p.PushContent(content);
                strcpy(content, values[i].at(paintMoveTableString[(int)PaintMoveTableLayout::MoveType]).get_ptr<const json::string_t*>()->c_str());
                p.PushContent(content);

            }

            return;
        }

        if (strcmp(typeString, chapterTableString[(int)ChapterTableLayout::Type]) == 0) {
            p.SetTableType(TableType::Chapter);
            char content[MAX_COLUMNS_CONTENT_LENGTH];
            for (int i = 0; i < values.size(); i++)
            {

                strcpy(content, values[i].at(chapterTableString[(int)ChapterTableLayout::Chapter]).get_ptr<const json::string_t*>()->c_str());
                p.PushContent(content);
                strcpy(content, values[i].at(chapterTableString[(int)ChapterTableLayout::Description]).get_ptr<const json::string_t*>()->c_str());
                p.PushContent(content);
            }

            return;
        }

        if (strcmp(typeString, sceneTableString[(int)SceneTableLayout::Type]) == 0) {
            p.SetTableType(TableType::Scene);
            char content[MAX_COLUMNS_CONTENT_LENGTH];
            for (int i = 0; i < values.size(); i++)
            {

                strcpy(content, values[i].at(sceneTableString[(int)SceneTableLayout::Scene]).get_ptr<const json::string_t*>()->c_str());
                p.PushContent(content);
                strcpy(content, values[i].at(sceneTableString[(int)SceneTableLayout::Description]).get_ptr<const json::string_t*>()->c_str());
                p.PushContent(content);
            }

            return;
        }

        if (strcmp(typeString, characterTableString[(int)CharacterTableLayout::Type]) == 0) {
            p.SetTableType(TableType::Character);
            char content[MAX_COLUMNS_CONTENT_LENGTH];
            for (int i = 0; i < values.size(); i++)
            {

                strcpy(content, values[i].at(characterTableString[(int)CharacterTableLayout::Character]).get_ptr<const json::string_t*>()->c_str());
                p.PushContent(content);
                strcpy(content, values[i].at(characterTableString[(int)CharacterTableLayout::Description]).get_ptr<const json::string_t*>()->c_str());
                p.PushContent(content);
            }

            return;
        }

        if (strcmp(typeString, pauseTableString[(int)PauseTableLayout::Type]) == 0) {
            p.SetTableType(TableType::Pause);
            char content[MAX_COLUMNS_CONTENT_LENGTH];
            for (int i = 0; i < values.size(); i++)
            {

                strcpy(content, values[i].at(pauseTableString[(int)PauseTableLayout::Pause]).get_ptr<const json::string_t*>()->c_str());
                p.PushContent(content);
                strcpy(content, values[i].at(pauseTableString[(int)PauseTableLayout::Time]).get_ptr<const json::string_t*>()->c_str());
                p.PushContent(content);
            }

            return;
        }

        if (strcmp(typeString, exhibitTableString[(int)ExhibitTableLayout::Type]) == 0) {
            p.SetTableType(TableType::Exhibit);
            char content[MAX_COLUMNS_CONTENT_LENGTH];
            for (int i = 0; i < values.size(); i++)
            {

                strcpy(content, values[i].at(exhibitTableString[(int)ExhibitTableLayout::Exhibit]).get_ptr<const json::string_t*>()->c_str());
                p.PushContent(content);
                strcpy(content, values[i].at(exhibitTableString[(int)ExhibitTableLayout::Description]).get_ptr<const json::string_t*>()->c_str());
                p.PushContent(content);
            }

            return;
        }

    }
    template<int column>
    void from_json(const json& j, StoryRow<column>& p)
    {

    }
}
#endif /* StoryTable_h */
