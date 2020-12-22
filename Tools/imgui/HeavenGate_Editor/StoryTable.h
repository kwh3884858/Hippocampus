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

#include "StoryRow.h"
#include "StoryTableDefine.h"
#include "StoryLayoutConstant.h"
#include "JsonUtility.h"

namespace HeavenGateEditor {
    using json = nlohmann::json;
    using std::vector;

    //enum class TableLayout :int;
    //enum class FontSizeTableLayout :int;

    template<int column, int MAX_CONTENT_LENGTH>
    class StoryTable {

    public:
        StoryTable();
        StoryTable(const StoryTable& storyTable);
        StoryTable(StoryTable&& storyTable);
        ~StoryTable();

        StoryTable& operator=(StoryTable&& storyTable)noexcept;

        bool PushName(const char* name);
        bool SetName(int index, const char* name);
        inline const char* GetName(int index)const;
        void UpdateName();

        bool PushContent(const char* content);
        bool SetContent(int rowIndex, int index, const char* content);
        inline const char* GetContent(int rowIndex, int index)const;
        char* GetContent(int rowIndex, int index);

        const StoryRow<column, MAX_CONTENT_LENGTH>* const GetRow(int index)const;
        StoryRow<column, MAX_CONTENT_LENGTH>* AddRow();
        StoryRow<column, MAX_CONTENT_LENGTH>* RemoveRow();
        void PushRow(StoryRow<column, MAX_CONTENT_LENGTH>* storyRow);

        int GetSize()const;
        void SetTableType(TableType tableType);
        TableType GetTableType()const;
        const char* GetHeaderName(int index) const;
    private:
        void Clear();

        StoryRow<column, MAX_CONTENT_LENGTH>* m_name;
        vector<StoryRow<column, MAX_CONTENT_LENGTH>*> m_content;
        TableType m_tableType;

    };



    template<int column, int MAX_CONTENT_LENGTH = MAX_COLUMNS_CONTENT_LENGTH>
    void to_json(json& j, const StoryTable<column, MAX_CONTENT_LENGTH>& p);

    template<int column, int MAX_CONTENT_LENGTH = MAX_COLUMNS_CONTENT_LENGTH>
    void from_json(const json& j, StoryTable<column, MAX_CONTENT_LENGTH>& p);



    //=======================Story Table========================

    template<int column, int MAX_CONTENT_LENGTH>
    StoryTable<column, MAX_CONTENT_LENGTH>::StoryTable()
    {
        m_name = new StoryRow<column, MAX_CONTENT_LENGTH>;
        m_tableType = TableType::None;
    }
    template<int column, int MAX_CONTENT_LENGTH>
    StoryTable<column, MAX_CONTENT_LENGTH>::StoryTable(const StoryTable& storyTable)
    {
        m_name(storyTable.m_name);
        for (int i = 0; i < m_content.size(); i++) {
            StoryRow<column, MAX_CONTENT_LENGTH>* tmpRow = new StoryRow<column, MAX_CONTENT_LENGTH>(*(storyTable.m_content[i]));
            m_content.push_back(tmpRow);
        }

        m_tableType = storyTable.m_tableType;
    }

    template<int column, int MAX_CONTENT_LENGTH>
    StoryTable<column, MAX_CONTENT_LENGTH>::StoryTable(StoryTable&& storyTable) :
        m_tableType(storyTable.m_tableType)
    {
        m_name = storyTable.m_name;
        m_content = storyTable.m_content;

        storyTable.m_name = nullptr;
        storyTable.m_content.clear();
    }
    template<int column, int MAX_CONTENT_LENGTH>
    StoryTable<column, MAX_CONTENT_LENGTH>::~StoryTable()
    {
        Clear();
    }

    template<int column, int MAX_CONTENT_LENGTH>
    const char* StoryTable<column, MAX_CONTENT_LENGTH>::GetHeaderName(int index) const {
        const char(*tableString)[MAX_ENUM_LENGTH] = (const char(*)[MAX_ENUM_LENGTH]) GetLayoutString(m_tableType);
        return tableString[index];
    }


    template<int column, int MAX_CONTENT_LENGTH>
    void StoryTable<column, MAX_CONTENT_LENGTH>::Clear()
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

    template<int column, int MAX_CONTENT_LENGTH>
    StoryTable<column, MAX_CONTENT_LENGTH>& StoryTable<column, MAX_CONTENT_LENGTH>::operator=(StoryTable&& storyTable) noexcept
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

    template<int column, int MAX_CONTENT_LENGTH>
    bool StoryTable<column, MAX_CONTENT_LENGTH>::PushName(const char* name)
    {
        return m_name->Push(name);
    }

    template<int column, int MAX_CONTENT_LENGTH>
    bool StoryTable<column, MAX_CONTENT_LENGTH>::SetName(int index, const char* name)
    {
        return m_name->Set(index, name);
    }

    template<int column, int MAX_CONTENT_LENGTH>
    bool StoryTable<column, MAX_CONTENT_LENGTH>::PushContent(const char* content)
    {
        StoryRow<column, MAX_CONTENT_LENGTH>* aRow;

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

    template<int column, int MAX_CONTENT_LENGTH>
    bool StoryTable<column, MAX_CONTENT_LENGTH>::SetContent(int row, int index, const char* content)
    {

        if (row < 0 || row >= m_content.size())
        {
            return false;
        }

        StoryRow<column, MAX_CONTENT_LENGTH>* aRow = m_content[row];
        if (index >= aRow->Size() || index < 0)
        {
            return false;
        }
        aRow->Set(index, content);

        return true;
    }

    template<int column, int MAX_CONTENT_LENGTH>
    const char* StoryTable<column, MAX_CONTENT_LENGTH>::GetName(int index)const
    {
        if (m_name == nullptr || index < 0)
        {
            return nullptr;
        }
        return m_name->Get(index);
    }

    template<int column, int MAX_CONTENT_LENGTH>
    void StoryTable<column, MAX_CONTENT_LENGTH>::UpdateName() {
        int layoutAmount = GetLayoutAmount(m_tableType);
        for (int i = 1; i <= layoutAmount; i++) {
            PushName(GetHeaderName(i));
        }
    }

    template<int column, int MAX_CONTENT_LENGTH>
    const char* StoryTable<column, MAX_CONTENT_LENGTH>::GetContent(int row, int index)const
    {
        if (row < 0 || row >= m_content.size())
        {
            return nullptr;
        }

        StoryRow<column, MAX_CONTENT_LENGTH>* aRow = m_content[row];

        return aRow->Get(index);
    }

    template<int column, int MAX_CONTENT_LENGTH>
    char* StoryTable<column, MAX_CONTENT_LENGTH>::GetContent(int rowIndex, int index) {
        return const_cast<char*>(const_cast<const StoryTable<column, MAX_CONTENT_LENGTH>*>(this)->GetContent(rowIndex, index));
    }


    template<int column, int MAX_CONTENT_LENGTH>
    const StoryRow<column, MAX_CONTENT_LENGTH>* const HeavenGateEditor::StoryTable<column, MAX_CONTENT_LENGTH>::GetRow(int index) const
    {
        return m_content[index];
    }

    template<int column, int MAX_CONTENT_LENGTH>
    int HeavenGateEditor::StoryTable<column, MAX_CONTENT_LENGTH>::GetSize() const
    {
        return m_content.size();
    }

    template<int column, int MAX_CONTENT_LENGTH>
    StoryRow<column, MAX_CONTENT_LENGTH>* HeavenGateEditor::StoryTable<column, MAX_CONTENT_LENGTH>::AddRow()
    {
        StoryRow<column, MAX_CONTENT_LENGTH>* newRow = new StoryRow<column, MAX_CONTENT_LENGTH>;
        PushRow(newRow);
        return newRow;
    }

    template<int column, int MAX_CONTENT_LENGTH>
    HeavenGateEditor::StoryRow<column, MAX_CONTENT_LENGTH>* HeavenGateEditor::StoryTable<column, MAX_CONTENT_LENGTH>::RemoveRow()
    {
        StoryRow<column, MAX_CONTENT_LENGTH>* row = m_content.back();
        m_content.pop_back();
        return row;
    }

    template<int column, int MAX_CONTENT_LENGTH>
    void HeavenGateEditor::StoryTable<column, MAX_CONTENT_LENGTH>::PushRow(StoryRow<column, MAX_CONTENT_LENGTH>* storyRow)
    {
        m_content.push_back(storyRow);
    }


    template<int column, int MAX_CONTENT_LENGTH>
    void HeavenGateEditor::StoryTable<column, MAX_CONTENT_LENGTH>::SetTableType(TableType tableType)
    {
        m_tableType = tableType;
    }

    template<int column, int MAX_CONTENT_LENGTH>
    TableType StoryTable<column, MAX_CONTENT_LENGTH>::GetTableType() const
    {
        return m_tableType;
    }

 
    //========================Json==========================

    template<int column, int MAX_CONTENT_LENGTH >
    void to_json(json& j, const StoryTable<column, MAX_CONTENT_LENGTH>& p) {

        const StoryRow<column, MAX_CONTENT_LENGTH>* tmp;

        if (column == 0) {
            j[tableString[(int)TableLayout::Header]] = json::array();
        }
        for (int i = 0; i < column; i++) {
            j[tableString[(int)TableLayout::Header]].push_back(p.GetName(i));
        }
        if (p.GetSize() == 0)
        {
            j[tableString[(int)TableLayout::Value]] = json::array();
        }

        switch (p.GetTableType())
        {

        case TableType::Font_Size:
        {
            j[tableString[(int)TableLayout::Type]] = fontSizeTableString[(int)FontSizeTableLayout::Type];
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

        case TableType::TachieMove:
        {
            j[tableString[(int)TableLayout::Type]] = tachieMoveTableString[(int)TachieMoveTableLayout::Type];
            for (int i = 0; i < p.GetSize(); i++)
            {
                tmp = p.GetRow(i);
                j[tableString[(int)TableLayout::Value]].push_back(json{
                    {tachieMoveTableString[(int)TachieMoveTableLayout::MoveAlias],            tmp->Get(MappingLayoutToArrayIndex((int)TachieMoveTableLayout::MoveAlias)) },
                    {tachieMoveTableString[(int)TachieMoveTableLayout::StartPointAlias],           tmp->Get(MappingLayoutToArrayIndex((int)TachieMoveTableLayout::StartPointAlias)) },
                    {tachieMoveTableString[(int)TachieMoveTableLayout::EndPointAlias],             tmp->Get(MappingLayoutToArrayIndex((int)TachieMoveTableLayout::EndPointAlias)) },
                    {tachieMoveTableString[(int)TachieMoveTableLayout::MoveCurve],            tmp->Get(MappingLayoutToArrayIndex((int)TachieMoveTableLayout::MoveCurve)) },
                    {tachieMoveTableString[(int)TachieMoveTableLayout::Duration],             tmp->Get(MappingLayoutToArrayIndex((int)TachieMoveTableLayout::Duration)) }
                    });
            }
            break;
        }

        //        case TableType::Chapter:
        //        {
        //            j[tableString[(int)TableLayout::Type]] = chapterTableString[(int)ChapterTableLayout::Type];
        //            for (int i = 0; i < p.GetSize(); i++)
        //            {
        //                tmp = p.GetRow(i);
        //                j[tableString[(int)TableLayout::Value]].push_back(json{
        //                    {chapterTableString[(int)ChapterTableLayout::Chapter],          tmp->Get(0) },
        //                    {chapterTableString[(int)ChapterTableLayout::Description],          tmp->Get(1) }
        //                });
        //            }
        //            break;
        //        }
        //
        //        case TableType::Scene:
        //        {
        //            j[tableString[(int)TableLayout::Type]] = sceneTableString[(int)SceneTableLayout::Type];
        //            for (int i = 0; i < p.GetSize(); i++)
        //            {
        //                tmp = p.GetRow(i);
        //                j[tableString[(int)TableLayout::Value]].push_back(json{
        //                    {sceneTableString[(int)SceneTableLayout::Scene],          tmp->Get(0) },
        //                    {sceneTableString[(int)SceneTableLayout::Description],          tmp->Get(1) }
        //                });
        //            }
        //            break;
        //        }

        case TableType::Character:
        {
            j[tableString[(int)TableLayout::Type]] = characterTableString[(int)CharacterTableLayout::Type];
            for (int i = 0; i < p.GetSize(); i++)
            {
                tmp = p.GetRow(i);
                j[tableString[(int)TableLayout::Value]].push_back(json{
                    {characterTableString[(int)CharacterTableLayout::CharacterName],          tmp->Get(0) },
                    {characterTableString[(int)CharacterTableLayout::TachieId],          tmp->Get(1) }
                    });
            }
            break;
        }

        case TableType::Pause:
        {
            j[tableString[(int)TableLayout::Type]] = pauseTableString[(int)PauseTableLayout::Type];
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
            for (int i = 0; i < p.GetSize(); i++)
            {
                tmp = p.GetRow(i);
                j[tableString[(int)TableLayout::Value]].push_back(json{
                    {exhibitTableString[(int)ExhibitTableLayout::ExhibitID],            tmp->Get(MappingLayoutToArrayIndex((int)ExhibitTableLayout::ExhibitID)) },
                    {exhibitTableString[(int)ExhibitTableLayout::Exhibit],              tmp->Get(MappingLayoutToArrayIndex((int)ExhibitTableLayout::Exhibit)) },
                    {exhibitTableString[(int)ExhibitTableLayout::Description],          tmp->Get(MappingLayoutToArrayIndex((int)ExhibitTableLayout::Description)) },
                    {exhibitTableString[(int)ExhibitTableLayout::ExhibitImageName],     tmp->Get(MappingLayoutToArrayIndex((int)ExhibitTableLayout::ExhibitImageName)) },
                    });
            }
            break;
        }

        case TableType::Effect:
        {
            j[tableString[(int)TableLayout::Type]] = effectTableString[(int)EffectTableLayout::Type];
            for (int i = 0; i < p.GetSize(); i++)
            {
                tmp = p.GetRow(i);
                j[tableString[(int)TableLayout::Value]].push_back(json{
                    {effectTableString[(int)EffectTableLayout::Effect],          tmp->Get(0) },
                    {effectTableString[(int)EffectTableLayout::Description],          tmp->Get(1) }
                    });
            }
            break;
        }

        case TableType::Bgm:
        {
            j[tableString[(int)TableLayout::Type]] = bgmTableString[(int)BgmTableLayout::Type];
            for (int i = 0; i < p.GetSize(); i++)
            {
                tmp = p.GetRow(i);
                j[tableString[(int)TableLayout::Value]].push_back(json{
                    {bgmTableString[(int)BgmTableLayout::Bgm],                  tmp->Get(MappingLayoutToArrayIndex((int)BgmTableLayout::Bgm))},
                    {bgmTableString[(int)BgmTableLayout::FileName],          tmp->Get(MappingLayoutToArrayIndex((int)BgmTableLayout::FileName))},
                    {bgmTableString[(int)BgmTableLayout::Volume],                tmp->Get(MappingLayoutToArrayIndex((int)BgmTableLayout::Volume))}
                    });
            }
            break;
        }

        case TableType::Tachie:
        {
            j[tableString[(int)TableLayout::Type]] = tachieTableString[(int)TachieTableLayout::Type];
            for (int i = 0; i < p.GetSize(); i++)
            {
                tmp = p.GetRow(i);
                j[tableString[(int)TableLayout::Value]].push_back(json{
                    {tachieTableString[(int)TachieTableLayout::Alias],             tmp->Get(0) },
                    {tachieTableString[(int)TachieTableLayout::FileName],          tmp->Get(1) }
                    });
            }
            break;
        }

        case TableType::Tachie_Position:
        {
            j[tableString[(int)TableLayout::Type]] = tachiePositionTableString[(int)TachiePositionTableLayout::Type];
            for (int i = 0; i < p.GetSize(); i++) {
                tmp = p.GetRow(i);
                j[tableString[(int)TableLayout::Value]].push_back(json{
                    {tachiePositionTableString[(int)TachiePositionTableLayout::Alias],          tmp->Get(0) },
                    {tachiePositionTableString[(int)TachiePositionTableLayout::PositionX],      tmp->Get(1) },
                    {tachiePositionTableString[(int)TachiePositionTableLayout::PositionY],      tmp->Get(2) }
                    });
            }
            break;
        }
        case TableType::Position:
        {
            j[tableString[(int)TableLayout::Type]] = positionTableString[(int)PositionTableLayout::Type];
            for (int i = 0; i < p.GetSize(); i++) {
                tmp = p.GetRow(i);
                j[tableString[(int)TableLayout::Value]].push_back(json{
                    {positionTableString[(int)PositionTableLayout::PositionAlias],          tmp->Get(0) },
                    {positionTableString[(int)PositionTableLayout::X],                      tmp->Get(1) },
                    {positionTableString[(int)PositionTableLayout::Y],                      tmp->Get(2) },
                    {positionTableString[(int)PositionTableLayout::Z],                      tmp->Get(3) }
                    });
            }
            break;
        }
        case TableType::Rotation:
        {
            j[tableString[(int)TableLayout::Type]] = rotationTableString[(int)RotationTableLayout::Type];
            for (int i = 0; i < p.GetSize(); i++) {
                tmp = p.GetRow(i);
                j[tableString[(int)TableLayout::Value]].push_back(json{
                    {rotationTableString[(int)RotationTableLayout::RotationAlias],          tmp->Get(0) },
                    {rotationTableString[(int)RotationTableLayout::X],                      tmp->Get(1) },
                    {rotationTableString[(int)RotationTableLayout::Y],                      tmp->Get(2) },
                    {rotationTableString[(int)RotationTableLayout::Z],                      tmp->Get(3) }
                    });
            }
            break;
        }

        default:
        {
            printf("Can not find suitable serialization function to json for %s", TableTypeString[(int)p.GetTableType()]);
            break;
        }

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


    template<int column, int MAX_CONTENT_LENGTH>
    void from_json(const json& j, StoryTable<column, MAX_CONTENT_LENGTH>& p)
    {
        char typeString[MAX_ENUM_LENGTH];
        GetCharPointerException(typeString, j, tableString[(int)TableLayout::Type]);
        json headers;
        json values;
        GetJsonException(headers, j, tableString[(int)TableLayout::Header]);
        GetJsonException(values, j, tableString[(int)TableLayout::Value]);

        /*get_ptr<const json::string_t *>()->c_str();*/

        if (strcmp(typeString, fontSizeTableString[(int)FontSizeTableLayout::Type]) == 0) {
            p.SetTableType(TableType::Font_Size);
            //char header[MAX_COLUMNS_CONTENT_LENGTH];
            //if (!headers.is_null())
            //{
            //    for (int i = 0; i < headers.size(); i++) {
            //        strcpy(header, headers[i].get_ptr<const json::string_t*>()->c_str());
            //        p.PushName(header);
            //    }
            //}
            //else {
                for (int i = 1; i < (int)FontSizeTableLayout::Amount; i++) {
                    p.PushName(fontSizeTableString[i]);
                }
            //}
            char content[MAX_COLUMNS_CONTENT_LENGTH];
            for (int i = 0; i < values.size(); i++)
            {
                GetCharPointerException(content, values[i], fontSizeTableString[(int)FontSizeTableLayout::Alias]);
                p.PushContent(content);
                GetCharPointerException(content, values[i], fontSizeTableString[(int)FontSizeTableLayout::Size]);
                p.PushContent(content);
            }
            return;
        }

        if (strcmp(typeString, colorTableString[(int)ColorTableLayout::Type]) == 0) {
            p.SetTableType(TableType::Color);
            //char header[MAX_COLUMNS_CONTENT_LENGTH];
            //if (!headers.is_null())
            //{
            //    for (int i = 0; i < headers.size(); i++) {
            //        strcpy(header, headers[i].get_ptr<const json::string_t*>()->c_str());
            //        p.PushName(header);
            //    }
            //}
            //else {
                for (int i = 1; i < (int)ColorTableLayout::Amount; i++) {
                    p.PushName(colorTableString[i]);
                }
            //}
            char content[MAX_COLUMNS_CONTENT_LENGTH];
            for (int i = 0; i < values.size(); i++)
            {
                GetCharPointerException(content, values[i], colorTableString[(int)ColorTableLayout::Alias]);
                p.PushContent(content);
                GetCharPointerException(content, values[i], colorTableString[(int)ColorTableLayout::R]);
                p.PushContent(content);
                GetCharPointerException(content, values[i], colorTableString[(int)ColorTableLayout::G]);
                p.PushContent(content);
                GetCharPointerException(content, values[i], colorTableString[(int)ColorTableLayout::B]);
                p.PushContent(content);
                GetCharPointerException(content, values[i], colorTableString[(int)ColorTableLayout::A]);
                p.PushContent(content);
            }
            return;
        }

        if (strcmp(typeString, tipTableString[(int)TipTableLayout::Type]) == 0) {
            p.SetTableType(TableType::Tips);
            //char header[MAX_COLUMNS_CONTENT_LENGTH];
            //if (!headers.is_null())
            //{
            //    for (int i = 0; i < headers.size(); i++) {
            //        strcpy(header, headers[i].get_ptr<const json::string_t*>()->c_str());
            //        p.PushName(header);
            //    }
            //}
            //else {
                for (int i = 1; i < (int)TipTableLayout::Amount; i++) {
                    p.PushName(tipTableString[i]);
                }
            //}
            char content[TIP_TABLE_MAX_CONTENT];
            for (int i = 0; i < values.size(); i++)
            {
                GetCharPointerException(content, values[i], tipTableString[(int)TipTableLayout::Tip]);
                p.PushContent(content);
                GetCharPointerException(content, values[i], tipTableString[(int)TipTableLayout::Description]);
                p.PushContent(content);
            }
            return;
        }

        if (strcmp(typeString, tachieMoveTableString[(int)TachieMoveTableLayout::Type]) == 0) {
            p.SetTableType(TableType::TachieMove);
            //char header[MAX_COLUMNS_CONTENT_LENGTH];
            //if (!headers.is_null())
            //{
            //    for (int i = 0; i < headers.size(); i++) {
            //        strcpy(header, headers[i].get_ptr<const json::string_t*>()->c_str());
            //        p.PushName(header);
            //    }
            //}
            //else {
                for (int i = 1; i < (int)TachieMoveTableLayout::Amount; i++) {
                    p.PushName(tachieMoveTableString[i]);
                }
            //}
            char content[MAX_COLUMNS_CONTENT_LENGTH];
            for (int i = 0; i < values.size(); i++)
            {
                GetCharPointerException(content, values[i], tachieMoveTableString[(int)TachieMoveTableLayout::MoveAlias]);
                p.PushContent(content);
                GetCharPointerException(content, values[i], tachieMoveTableString[(int)TachieMoveTableLayout::StartPointAlias]);
                p.PushContent(content);
                GetCharPointerException(content, values[i], tachieMoveTableString[(int)TachieMoveTableLayout::EndPointAlias]);
                p.PushContent(content);
                GetCharPointerException(content, values[i], tachieMoveTableString[(int)TachieMoveTableLayout::MoveCurve]);
                p.PushContent(content);
                GetCharPointerException(content, values[i], tachieMoveTableString[(int)TachieMoveTableLayout::Duration]);
                p.PushContent(content);
            }
            return;
        }
        //
        //    if (strcmp(typeString, chapterTableString[(int)ChapterTableLayout::Type]) == 0) {
        //        p.SetTableType(TableType::Chapter);
        //        char header[MAX_COLUMNS_CONTENT_LENGTH];
        //        if(!headers.is_null())
        //        {
        //            for(int i = 0; i < headers.size(); i++){
        //                strcpy(header, headers[i].get_ptr<const json::string_t*>()->c_str());
        //                p.PushName(header);
        //            }
        //        }else{
        //            for (int i = 1; i < (int)ChapterTableLayout::Amount; i++) {
        //                p.PushName(chapterTableString[i]);
        //            }
        //        }
        //        char content[MAX_COLUMNS_CONTENT_LENGTH];
        //        for (int i = 0; i < values.size(); i++)
        //        {
        //            GetContentException(content, values[i], chapterTableString[(int)ChapterTableLayout::Chapter]);
        //            p.PushContent(content);
        //            GetContentException(content, values[i], chapterTableString[(int)ChapterTableLayout::Description]);
        //            p.PushContent(content);
        //        }
        //        return;
        //    }
        //
        //    if (strcmp(typeString, sceneTableString[(int)SceneTableLayout::Type]) == 0) {
        //        p.SetTableType(TableType::Scene);
        //        char header[MAX_COLUMNS_CONTENT_LENGTH];
        //        if(!headers.is_null())
        //        {
        //            for(int i = 0; i < headers.size(); i++){
        //                strcpy(header, headers[i].get_ptr<const json::string_t*>()->c_str());
        //                p.PushName(header);
        //            }
        //        }else{
        //            for (int i = 1; i < (int)SceneTableLayout::Amount; i++) {
        //                p.PushName(sceneTableString[i]);
        //            }
        //        }
        //        char content[MAX_COLUMNS_CONTENT_LENGTH];
        //        for (int i = 0; i < values.size(); i++)
        //        {
        //            GetContentException(content, values[i], sceneTableString[(int)SceneTableLayout::Scene]);
        //            p.PushContent(content);
        //            GetContentException(content, values[i], sceneTableString[(int)SceneTableLayout::Description]);
        //            p.PushContent(content);
        //        }
        //        return;
        //    }

        if (strcmp(typeString, characterTableString[(int)CharacterTableLayout::Type]) == 0) {
            p.SetTableType(TableType::Character);
            //char header[MAX_COLUMNS_CONTENT_LENGTH];
            //if (!headers.is_null())
            //{
            //    for (int i = 0; i < headers.size(); i++) {
            //        strcpy(header, headers[i].get_ptr<const json::string_t*>()->c_str());
            //        p.PushName(header);
            //    }
            //}
            //else {
                for (int i = 1; i < (int)CharacterTableLayout::Amount; i++) {
                    p.PushName(characterTableString[i]);
                }
            //}
            char content[MAX_COLUMNS_CONTENT_LENGTH];
            for (int i = 0; i < values.size(); i++)
            {
                GetCharPointerException(content, values[i], characterTableString[(int)CharacterTableLayout::CharacterName]);
                p.PushContent(content);
                GetCharPointerException(content, values[i], characterTableString[(int)CharacterTableLayout::TachieId]);
                p.PushContent(content);
            }
            return;
        }

        if (strcmp(typeString, pauseTableString[(int)PauseTableLayout::Type]) == 0) {
            p.SetTableType(TableType::Pause);
            //char header[MAX_COLUMNS_CONTENT_LENGTH];
            //if (!headers.is_null())
            //{
            //    for (int i = 0; i < headers.size(); i++) {
            //        strcpy(header, headers[i].get_ptr<const json::string_t*>()->c_str());
            //        p.PushName(header);
            //    }
            //}
            //else {
                for (int i = 1; i < (int)PauseTableLayout::Amount; i++) {
                    p.PushName(pauseTableString[i]);
                }
            //}
            char content[MAX_COLUMNS_CONTENT_LENGTH];
            for (int i = 0; i < values.size(); i++)
            {
                GetCharPointerException(content, values[i], pauseTableString[(int)PauseTableLayout::Pause]);
                p.PushContent(content);
                GetCharPointerException(content, values[i], pauseTableString[(int)PauseTableLayout::Time]);
                p.PushContent(content);
            }
            return;
        }

        if (strcmp(typeString, exhibitTableString[(int)ExhibitTableLayout::Type]) == 0) {
            p.SetTableType(TableType::Exhibit);
            //char header[MAX_COLUMNS_CONTENT_LENGTH];
            //if (!headers.is_null() && headers.size() == (int)ExhibitTableLayout::Amount)
            //{
            //    for (int i = 0; i < headers.size(); i++) {
            //        strcpy(header, headers[i].get_ptr<const json::string_t*>()->c_str());
            //        p.PushName(header);
            //    }
            //}
            //else {
                for (int i = 1; i < (int)ExhibitTableLayout::Amount; i++) {
                    p.PushName(exhibitTableString[i]);
                }
            //}
            char content[EXHIBIT_TABLE_MAX_CONTENT];
            for (int i = 0; i < values.size(); i++)
            {
                GetCharPointerException(content, values[i], exhibitTableString[(int)ExhibitTableLayout::ExhibitID]);
                p.PushContent(content);
                GetCharPointerException(content, values[i], exhibitTableString[(int)ExhibitTableLayout::Exhibit]);
                p.PushContent(content);
                GetCharPointerException(content, values[i], exhibitTableString[(int)ExhibitTableLayout::Description]);
                p.PushContent(content);
                GetCharPointerException(content, values[i], exhibitTableString[(int)ExhibitTableLayout::ExhibitImageName]);
                p.PushContent(content);
            }
            return;
        }

        if (strcmp(typeString, effectTableString[(int)EffectTableLayout::Type]) == 0) {
            p.SetTableType(TableType::Effect);
            //char header[MAX_COLUMNS_CONTENT_LENGTH];
            //if (!headers.is_null())
            //{
            //    for (int i = 0; i < headers.size(); i++) {
            //        strcpy(header, headers[i].get_ptr<const json::string_t*>()->c_str());
            //        p.PushName(header);
            //    }
            //}
            //else {
                for (int i = 1; i < (int)EffectTableLayout::Amount; i++) {
                    p.PushName(effectTableString[i]);
                }
            //}
            char content[MAX_COLUMNS_CONTENT_LENGTH];
            for (int i = 0; i < values.size(); i++)
            {
                GetCharPointerException(content, values[i], effectTableString[(int)EffectTableLayout::Effect]);
                p.PushContent(content);
                GetCharPointerException(content, values[i], effectTableString[(int)EffectTableLayout::Description]);
                p.PushContent(content);
            }
            return;
        }

        if (strcmp(typeString, bgmTableString[(int)BgmTableLayout::Type]) == 0) {
            p.SetTableType(TableType::Bgm);
            //char header[MAX_COLUMNS_CONTENT_LENGTH];
            //if (!headers.is_null())
            //{
            //    for (int i = 0; i < headers.size(); i++) {
            //        strcpy(header, headers[i].get_ptr<const json::string_t*>()->c_str());
            //        p.PushName(header);
            //    }
            //}
            //else {
                for (int i = 1; i < (int)BgmTableLayout::Amount; i++) {
                    p.PushName(bgmTableString[i]);
                }
            //}
            char content[MAX_COLUMNS_CONTENT_LENGTH];
            for (int i = 0; i < values.size(); i++)
            {
                GetCharPointerException(content, values[i], bgmTableString[(int)BgmTableLayout::Bgm]);
                p.PushContent(content);
                GetCharPointerException(content, values[i], bgmTableString[(int)BgmTableLayout::FileName]);
                p.PushContent(content);
                GetCharPointerException(content, values[i], bgmTableString[(int)BgmTableLayout::Volume]);
                p.PushContent(content);
                
            }
            return;
        }

        if (strcmp(typeString, tachieTableString[(int)TachieTableLayout::Type]) == 0) {
            p.SetTableType(TableType::Tachie);
            //char header[MAX_COLUMNS_CONTENT_LENGTH];
            //if (!headers.is_null())
            //{
            //    for (int i = 0; i < headers.size(); i++) {
            //        strcpy(header, headers[i].get_ptr<const json::string_t*>()->c_str());
            //        p.PushName(header);
            //    }
            //}
            //else {
                for (int i = 1; i < (int)TachieTableLayout::Amount; i++) {
                    p.PushName(tachieTableString[i]);
                }
            //}
            char content[MAX_COLUMNS_CONTENT_LENGTH];
            for (int i = 0; i < values.size(); i++)
            {
                GetCharPointerException(content, values[i], tachieTableString[(int)TachieTableLayout::Alias]);
                p.PushContent(content);
                GetCharPointerException(content, values[i], tachieTableString[(int)TachieTableLayout::FileName]);
                p.PushContent(content);
            }
            return;
        }

        if (strcmp(typeString, tachiePositionTableString[(int)TachiePositionTableLayout::Type]) == 0)
        {
            p.SetTableType(TableType::Tachie_Position);
            //char header[MAX_COLUMNS_CONTENT_LENGTH];
            //if (!headers.is_null())
            //{
            //    for (int i = 0; i < headers.size(); i++) {
            //        strcpy(header, headers[i].get_ptr<const json::string_t*>()->c_str());
            //        p.PushName(header);
            //    }
            //}
            //else {
                for (int i = 1; i < (int)TachiePositionTableLayout::Amount; i++) {
                    p.PushName(tachiePositionTableString[i]);
                }
            //}
            char content[MAX_COLUMNS_CONTENT_LENGTH];
            for (int i = 0; i < values.size(); i++)
            {
                GetCharPointerException(content, values[i], tachiePositionTableString[(int)TachiePositionTableLayout::Alias]);
                p.PushContent(content);
                GetCharPointerException(content, values[i], tachiePositionTableString[(int)TachiePositionTableLayout::PositionX]);
                p.PushContent(content);
                GetCharPointerException(content, values[i], tachiePositionTableString[(int)TachiePositionTableLayout::PositionY]);
                p.PushContent(content);
            }
            return;
        }
        if (strcmp(typeString, positionTableString[(int)PositionTableLayout::Type]) == 0)
        {
            p.SetTableType(TableType::Position);
            for (int i = 1; i < (int)PositionTableLayout::Amount; i++) {
                p.PushName(positionTableString[i]);
            }
            char content[MAX_COLUMNS_CONTENT_LENGTH];
            for (int i = 0; i < values.size(); i++)
            {
                GetCharPointerException(content, values[i], positionTableString[(int)PositionTableLayout::PositionAlias]);
                p.PushContent(content);
                GetCharPointerException(content, values[i], positionTableString[(int)PositionTableLayout::X]);
                p.PushContent(content);
                GetCharPointerException(content, values[i], positionTableString[(int)PositionTableLayout::Y]);
                p.PushContent(content);
                GetCharPointerException(content, values[i], positionTableString[(int)PositionTableLayout::Z]);
                p.PushContent(content);
            }
            return;
        }
        if (strcmp(typeString, rotationTableString[(int)RotationTableLayout::Type]) == 0)
        {
            p.SetTableType(TableType::Position);
            for (int i = 1; i < (int)RotationTableLayout::Amount; i++) {
                p.PushName(rotationTableString[i]);
            }
            char content[MAX_COLUMNS_CONTENT_LENGTH];
            for (int i = 0; i < values.size(); i++)
            {
                GetCharPointerException(content, values[i], rotationTableString[(int)RotationTableLayout::RotationAlias]);
                p.PushContent(content);
                GetCharPointerException(content, values[i], rotationTableString[(int)RotationTableLayout::X]);
                p.PushContent(content);
                GetCharPointerException(content, values[i], rotationTableString[(int)RotationTableLayout::Y]);
                p.PushContent(content);
                GetCharPointerException(content, values[i], rotationTableString[(int)RotationTableLayout::Z]);
                p.PushContent(content);
            }
            return;
        }
    }

}
#endif /* StoryTable_h */
