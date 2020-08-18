#include "HeavenGateWindowPreview.h"

#include "StoryJsonWordNode.h"
#include "StoryJson.h"

#include "StoryJsonContentCompiler.h"

#include "HeavenGateEditorUtility.h"

#include "StoryTable.h"
#include "StoryTableManager.h"
#include "StoryColor.h"

#include <deque>
#include "imgui.h"

using std::deque;
namespace HeavenGateEditor {

    void HeavenGateWindowPreview::Initialize() {
        m_compiledWord = new StoryWord;

        memset(m_compiledWord->m_name, '\0', MAX_NAME);
        memset(m_compiledWord->m_content, '\0', MAX_CONTENT);
    }
    void HeavenGateWindowPreview::Shutdown() {
        if (m_compiledWord != nullptr) {
            delete m_compiledWord;
        }
        m_compiledWord = nullptr;
    }


    void HeavenGateWindowPreview::UpdateMainWindow() {

        vector<StoryJsonContentCompiler::Token*>tokens = StoryJsonContentCompiler::Instance().CompileToTokens(m_compiledWord);

        TableType currentState;
        deque<TableType> editorState;
        bool isReadCloseLabel;

        isReadCloseLabel = false;
        currentState = TableType::None;

        //Tmp value
        ImVec4 color(1.0f, 1.0f, 1.0f, 1.0f);

        char tmpTip[MAX_COLUMNS_CONTENT_LENGTH];
        memset(tmpTip, '\0', MAX_COLUMNS_CONTENT_LENGTH);

        char tmpExhibit[EXHIBIT_TABLE_MAX_CONTENT];
        memset(tmpExhibit, '\0', EXHIBIT_TABLE_MAX_CONTENT);

        char tmpBgm[MAX_COLUMNS_CONTENT_LENGTH];
        memset(tmpBgm, '\0', MAX_COLUMNS_CONTENT_LENGTH);

        char tmpEffect[MAX_COLUMNS_CONTENT_LENGTH];
        memset(tmpEffect, '\0', MAX_COLUMNS_CONTENT_LENGTH);

        char tmpTachieCommand[NUM_OF_TACHIE_COMMAND][MAX_COLUMNS_CONTENT_LENGTH];
        memset(tmpTachieCommand, '\0', NUM_OF_TACHIE_COMMAND * MAX_COLUMNS_CONTENT_LENGTH);

        char tmpFontSize[MAX_COLUMNS_CONTENT_LENGTH];
        memset(tmpFontSize, '\0', MAX_COLUMNS_CONTENT_LENGTH);

        char tmpPause[MAX_COLUMNS_CONTENT_LENGTH];
        memset(tmpPause, '\0', MAX_COLUMNS_CONTENT_LENGTH);

        bool isExhibit = false;

        //static float wrap_width = 200.0f;
        //ImGui::SliderFloat("Wrap width", &wrap_width, -20, 600, "%.0f");
        


        ImGui::Text("Preview:");
        ImGui::Text(m_compiledWord->m_name);
        ImGui::Separator();

//        ImGui::PushTextWrapPos(ImGui::GetCursorPos().x + ImGui::GetItemRectSize().x - 5);

        for (auto iter = tokens.cbegin(); iter != tokens.end(); iter++)
        {
            if ((*iter)->m_tokeType == StoryJsonContentCompiler::TokenType::TokenContent)
            {
                ImGui::PushTextWrapPos(ImGui::GetCursorPos().x + ImGui::GetWindowWidth() - 15);
                ImGui::TextColored(color, (*iter)->m_content);
//                ImGui::SameLine(0, 0);
                ImGui::PopTextWrapPos();

            }
            else if ((*iter)->m_tokeType == StoryJsonContentCompiler::TokenType::TokenInstructor)
            {
                if (isReadCloseLabel)
                {
                    // Is closing state
                    if (editorState.empty())
                    {
                        printf("Close Label is lack\n");
                        continue;
                    }

                    currentState = editorState.back();
                    editorState.pop_back();
                    isReadCloseLabel = false;

                    switch (currentState)
                    {
                    case HeavenGateEditor::TableType::None:
                        break;
                    case HeavenGateEditor::TableType::Font_Size:
                        if (strlen(tmpFontSize) != 0) {
                            ImGui::TextColored(colorGreen, "[End font size]");

                        }
                        else {
                            ImGui::TextColored(colorRed, "!!Can not find font size");
                        }
                        memset(tmpFontSize, '\0', MAX_COLUMNS_CONTENT_LENGTH);
//                        ImGui::SameLine(0, 0);
                        break;
                    case HeavenGateEditor::TableType::Color:
                        color = colorWhite;
                        break;
                    case HeavenGateEditor::TableType::Tips:
                        ImGui::TextColored(colorGreen, "[Show Tip Here: %s]", tmpTip);
                        memset(tmpTip, '\0', MAX_COLUMNS_CONTENT_LENGTH);
//                        ImGui::SameLine(0, 0);
                        break;
                    case HeavenGateEditor::TableType::Paint_Move:
                        break;
                    case HeavenGateEditor::TableType::Exhibit:
                        if (isExhibit == true) {
                            ImGui::TextColored(colorGreen, "[Get Exhibit Here: %s]", tmpExhibit);
                            isExhibit = false;
                        }
                        else {
                            ImGui::TextColored(colorRed, "!!Can not find exhibit ID", tmpExhibit);
                        }
                        memset(tmpExhibit, '\0', EXHIBIT_TABLE_MAX_CONTENT);
                        break;
                    case TableType::Pause:
                        if (strlen(tmpPause) != 0) {
                            ImGui::TextColored(colorGreen, "[End interval time]");
                        }
                        else {
                            ImGui::TextColored(colorRed, "!!Can not find interval time");
                        }
                        memset(tmpPause, '\0', MAX_COLUMNS_CONTENT_LENGTH);
//                        ImGui::SameLine(0, 0);
                        break;
                    case TableType::Bgm:
                        if (strlen(tmpBgm) != 0) {
                            ImGui::TextColored(colorGreen, "[Play Bgm: %s]", tmpBgm);

                        }
                        else {
                            ImGui::TextColored(colorRed, "!!Can not find bgm");
                        }
                        memset(tmpBgm, '\0', MAX_COLUMNS_CONTENT_LENGTH);
//                        ImGui::SameLine(0, 0);
                        break;
                    case TableType::Effect:
                        if (strlen(tmpEffect) != 0) {
                            ImGui::TextColored(colorGreen, "[Play Effect: %s]", tmpEffect);

                        }
                        else {
                            ImGui::TextColored(colorRed, "!!Can not find effect");
                        }
                        memset(tmpEffect, '\0', MAX_COLUMNS_CONTENT_LENGTH);

//                        ImGui::SameLine(0, 0);
                        break;
                    case TableType::Tachie:
                        if (strlen(tmpTachieCommand[0]) == 0) {
                            ImGui::TextColored(colorRed, "!!Tchie is empty");
                        }
                        if (strlen(tmpTachieCommand[1]) == 0) {
                            ImGui::TextColored(colorRed, "!!Tchie position is empty");
                        }
                        ImGui::TextColored(colorGreen, "[Display Tachie: %s] [Tachie Position: %s]", tmpTachieCommand[0], tmpTachieCommand[1]);
                        memset(tmpTachieCommand, '\0', NUM_OF_TACHIE_COMMAND * MAX_COLUMNS_CONTENT_LENGTH);

//                        ImGui::SameLine(0, 0);
                        break;
                    default:
                        break;
                    }
                }
                else
                {
                    // Is start state
                    if (strcmp((*iter)->m_content, fontSizeTableString[(int)FontSizeTableLayout::Type]) == 0)
                    {
                        editorState.push_back(TableType::Font_Size);
                    }
                    if (strcmp((*iter)->m_content, colorTableString[(int)FontSizeTableLayout::Type]) == 0)
                    {
                        editorState.push_back(TableType::Color);
                    }
                    if (strcmp((*iter)->m_content, paintMoveTableString[(int)FontSizeTableLayout::Type]) == 0)
                    {
                        editorState.push_back(TableType::Paint_Move);
                    }
                    if (strcmp((*iter)->m_content, pauseTableString[(int)FontSizeTableLayout::Type]) == 0)
                    {
                        editorState.push_back(TableType::Pause);
                    }
                    if (strcmp((*iter)->m_content, tipTableString[(int)TipTableLayout::Type]) == 0)
                    {
                        editorState.push_back(TableType::Tips);
                    }
                    if (strcmp((*iter)->m_content, bgmTableString[(int)BgmTableLayout::Type]) == 0) {
                        editorState.push_back(TableType::Bgm);
                    }
                    if (strcmp((*iter)->m_content, effectTableString[(int)EffectTableLayout::Type]) == 0) {
                        editorState.push_back(TableType::Effect);
                    }
                    if (strcmp((*iter)->m_content, exhibitTableString[(int)ExhibitTableLayout::Type]) == 0) {
                        editorState.push_back(TableType::Exhibit);
                    }
                    if (strcmp((*iter)->m_content, tachieTableString[(int)TachieTableLayout::Type]) == 0)
                    {
                        editorState.push_back(TableType::Tachie);
                    }
                }
            }
            else if ((*iter)->m_tokeType == StoryJsonContentCompiler::TokenType::TokenIdentity)
            {

                if (editorState.empty())
                {
                    printf("No Identity Exist. \n");
                    continue;
                }
                switch (editorState.back())
                {
                case HeavenGateEditor::TableType::None:
                    break;
                case HeavenGateEditor::TableType::Font_Size:
                {
                    const StoryTable<FONT_SIZE_MAX_COLUMN>* const fontSizeTable = StoryTableManager::Instance().GetFontSizeTable();
                    for (int i = 0; i < fontSizeTable->GetSize(); i++)
                    {
                        const StoryRow<FONT_SIZE_MAX_COLUMN>* const row = fontSizeTable->GetRow(i);
                        if (strcmp(row->Get(0), (*iter)->m_content) == 0)
                        {
                            strcpy(tmpFontSize, row->Get(1));
                        }
                    }
                    if (strlen(tmpFontSize) != 0) {
                        ImGui::TextColored(colorGreen, "[Start font size: %s]", tmpFontSize);
                    }
                    else {
                        ImGui::TextColored(colorRed, "Can not find font size");
                    }
                    ImGui::SameLine(0, 0);
                }
                break;
                case HeavenGateEditor::TableType::Color:
                {
                    char colorAlias[MAX_COLUMNS_CONTENT_LENGTH];
                    StoryTable<COLOR_MAX_COLUMN>* colorTable = StoryTableManager::Instance().GetColorTable();
                    for (int j = 0; j < colorTable->GetSize(); j++)
                    {
                        strcpy(colorAlias, colorTable->GetRow(j)->Get(0));
                        if (strcmp(colorAlias, (*iter)->m_content) == 0)
                        {
                            color = ImVec4(
                                atoi(colorTable->GetRow(j)->Get(1)),
                                atoi(colorTable->GetRow(j)->Get(2)),
                                atoi(colorTable->GetRow(j)->Get(3)),
                                atoi(colorTable->GetRow(j)->Get(4))
                            );
                            color = HeavenGateEditorUtility::ConvertRGBAToFloat4(color);
                        }

                    }

                    break;
                }
                case HeavenGateEditor::TableType::Exhibit: {
                    const StoryTable<EXHIBIT_COLUMN, EXHIBIT_TABLE_MAX_CONTENT>* const exhibitTable = StoryTableManager::Instance().GetExhibitTable();

                    for (int i = 0; i < exhibitTable->GetSize(); i++)
                    {
                        const StoryRow<EXHIBIT_COLUMN, EXHIBIT_TABLE_MAX_CONTENT>* const row = exhibitTable->GetRow(i);
                        if (strcmp(row->Get(0), (*iter)->m_content) == 0)
                        {
                            strcpy(tmpExhibit, row->Get(1));
                            isExhibit = true;
                        }
                    }
                    break;
                }
                case HeavenGateEditor::TableType::Tips:
                {
                    const StoryTable<TIP_MAX_COLUMN, TIP_TABLE_MAX_CONTENT>* const tipTable = StoryTableManager::Instance().GetTipTable();

                    for (int i = 0; i < tipTable->GetSize(); i++)
                    {
                        const StoryRow<TIP_MAX_COLUMN, TIP_TABLE_MAX_CONTENT>* const row = tipTable->GetRow(i);
                        if (strcmp(row->Get(0), (*iter)->m_content) == 0)
                        {
                            strcpy(tmpTip, row->Get(1));
                        }
                    }
                }
                break;
                case HeavenGateEditor::TableType::Paint_Move:
                    break;
                case TableType::Pause:
                {
                    const StoryTable<PAUSE_MAX_COLUMN>* const pauseTable = StoryTableManager::Instance().GetPauseTable();
                    for (int i = 0; i < pauseTable->GetSize(); i++)
                    {
                        const StoryRow<PAUSE_MAX_COLUMN>* const row = pauseTable->GetRow(i);
                        if (strcmp(row->Get(0), (*iter)->m_content) == 0)
                        {
                            strcpy(tmpPause, row->Get(1));
                        }
                    }
                    if (strlen(tmpPause) != 0) {
                        ImGui::TextColored(colorGreen, "[Start interval time between words : %s]", tmpPause);
                    }
                    else {
                        ImGui::TextColored(colorRed, "Can not find interval time");
                    }
                    ImGui::SameLine(0, 0);
                }
                break;
                case TableType::Bgm:
                {
                    const StoryTable<BGM_MAX_COLUMN>* const bgmTable = StoryTableManager::Instance().GetBgmTable();
                    for (int i = 0; i < bgmTable->GetSize(); i++)
                    {
                        const StoryRow<BGM_MAX_COLUMN>* const row = bgmTable->GetRow(i);
                        if (strcmp(row->Get(0), (*iter)->m_content) == 0)
                        {
                            const char* description =  row->Get(MappingLayoutToArrayIndex((int)BgmTableLayout::Description));
                           const char* volume = row->Get(MappingLayoutToArrayIndex((int)BgmTableLayout::Volume));
                            if (strlen(description) == 0) {
                                strcpy(tmpBgm, "No file name");
                            }else{
                                strcpy(tmpBgm, description);
                            }
                            if (strlen(volume) == 0) {
                                strcpy(tmpBgm, "No Volume");
                            }else{
                                strcat(tmpBgm, " | volume: ");
                                strcat(tmpBgm, volume);
                            }


                        }
                    }
                }
                break;

                case TableType::Effect:
                {
                    const StoryTable<EFFECT_MAX_COLUMN>* const effectTable = StoryTableManager::Instance().GetEffectTable();
                    for (int i = 0; i < effectTable->GetSize(); i++)
                    {
                        const StoryRow<EFFECT_MAX_COLUMN>* const row = effectTable->GetRow(i);
                        if (strcmp(row->Get(0), (*iter)->m_content) == 0)
                        {
                            strcpy(tmpEffect, row->Get(1));
                        }
                    }
                }
                break;
                case TableType::Tachie:
                {
                    IdOperator::ParseStringId<'+', MAX_COLUMNS_CONTENT_LENGTH, NUM_OF_TACHIE_COMMAND>((*iter)->m_content, tmpTachieCommand);

                    const StoryTable<TACHIE_MAX_COLUMN>* const tachieTable = StoryTableManager::Instance().GetTachieTable();
                    const StoryTable<TACHIE_POSITION_MAX_COLUMN>* const tachiePositionTable = StoryTableManager::Instance().GetTachiePositionTable();
                    for (int i = 0; i < tachieTable->GetSize(); i++)
                    {
                        const StoryRow<TACHIE_MAX_COLUMN>* const row = tachieTable->GetRow(i);
                        if (strcmp(row->Get(0), tmpTachieCommand[0]) == 0)
                        {
                            strcpy(tmpTachieCommand[0], row->Get(1));
                        }
                    }
                    for (int i = 0; i < tachiePositionTable->GetSize(); i++)
                    {
                        const StoryRow<TACHIE_POSITION_MAX_COLUMN>* const row = tachiePositionTable->GetRow(i);
                        if (strcmp(row->Get(0), tmpTachieCommand[1]) == 0)
                        {
                            strcpy(tmpTachieCommand[1], row->Get(1));
                            strcat(tmpTachieCommand[1], ",");
                            strcat(tmpTachieCommand[1], row->Get(2));
                        }
                    }

                }
                break;
                default:
                    break;
                }
            }
            else if ((*iter)->m_tokeType == StoryJsonContentCompiler::TokenType::TokenOpSlash)
            {
                //Start close
                isReadCloseLabel = true;
            }

        }

//        ImGui::GetWindowDrawList()->AddRect(ImGui::GetItemRectMin(), ImGui::GetItemRectMax(), IM_COL32(255, 255, 0, 255));
//        ImGui::PopTextWrapPos();

        ImGui::Text("");
        ImGui::Separator();
    }

    void HeavenGateWindowPreview::SetPreviewWord(const StoryWord& word) {
        *m_compiledWord = word;
    }

}
