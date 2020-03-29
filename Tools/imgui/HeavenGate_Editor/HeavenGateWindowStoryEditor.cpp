

#include "HeavenGateWindowStoryEditor.h"
#include "CharacterUtility.h"

#include "HeavenGateWindowSelectStory.h"
#include "HeavenGatePopupInputFileName.h"
#include "HeavenGateEditorUtility.h"
#include "StoryTimer.h"

#include "StoryJsonContentCompiler.h"
#include "StoryJsonManager.h"
#include "StoryJson.h"
#include "StoryJsonWordNode.h"
#include "StoryJsonLabelNode.h"
#include "StoryJsonJumpNode.h"
#include "StoryJsonExhibitNode.h"


#include "StoryTable.h"
#include "StoryFileManager.h"
#include "StoryTableManager.h"

#include <iostream>

#include <stdio.h>


namespace HeavenGateEditor {


    HeavenGateWindowStoryEditor::HeavenGateWindowStoryEditor()
    {
    }

    HeavenGateWindowStoryEditor::~HeavenGateWindowStoryEditor()
    {
   
    }

    void HeavenGateWindowStoryEditor::Initialize()
    {
        m_storyJson = nullptr;
        m_selectStoryWindow = new HeavenGateWindowSelectStory();
        m_selectStoryWindow->Initialize();

        m_inputFileNamePopup = new HeavenGatePopupInputFileName();
        m_inputFileNamePopup->Initialize();

        //Default open select story window
        bool* selectStoryWindowHandle = m_selectStoryWindow->GetHandle();
        *selectStoryWindowHandle = true;

        memset(m_notification, '\0', sizeof(m_notification));

        //Auto save callback
        StoryTimer<HeavenGateWindowStoryEditor>::AddCallback(60 * 1, this, &HeavenGateWindowStoryEditor::AutoSaveCallback);

    }

    void HeavenGateWindowStoryEditor::Shutdown()
    {
        if (m_selectStoryWindow != nullptr) {
            m_selectStoryWindow->Shutdown();
            delete m_selectStoryWindow;
        }
        m_selectStoryWindow = nullptr;

        if (m_inputFileNamePopup != nullptr) {
            m_inputFileNamePopup->Shutdown();
            delete m_inputFileNamePopup;
        }
        m_inputFileNamePopup = nullptr;

        m_storyJson = nullptr;

        memset(m_notification, '\0', sizeof(m_notification));
    }

    void HeavenGateWindowStoryEditor::UpdateMainWindow()
    {
        m_selectStoryWindow->Update();

        //For save as new file
        m_inputFileNamePopup->Update();

        m_storyJson = StoryJsonManager::Instance().GetStoryJson();
        if (m_storyJson == nullptr)
        {
            return;
        }

        ImGui::Text("Heaven Gate says hello. (%s)", IMGUI_VERSION);
        ImGui::Text(m_notification);
        ImGui::Spacing();
        if (m_storyJson != nullptr &&
            m_storyJson->IsExistFullPath() == true) {
            ImGui::Text("Current story path: %s", m_storyJson->GetFullPath());

        }

        StoryTable<CHAPTER_COLUMN>* const chapterTable = StoryTableManager::Instance().GetChapterTable();
        StoryTable<SCENE_COLUMN>* const sceneTable = StoryTableManager::Instance().GetSceneTable();
        if (chapterTable == nullptr || sceneTable == nullptr)
        {
            return;
        }

        if (ImGui::BeginCombo("Chapter", m_storyJson->GetChapter(), 0)) // The second parameter is the label previewed before opening the combo.
        {

            for (int i = 0; i < chapterTable->GetSize(); i++)
            {
                bool is_selected = strcmp(m_storyJson->GetChapter(), chapterTable->GetRow(i)->Get(0)) == 0;
                if (ImGui::Selectable(chapterTable->GetRow(i)->Get(0), is_selected))
                    m_storyJson->SetChapter(chapterTable->GetRow(i)->Get(0));
                if (is_selected)
                    ImGui::SetItemDefaultFocus();   // Set the initial focus when opening the combo (scrolling + for keyboard navigation support in the upcoming navigation branch)


            }

            ImGui::EndCombo();
        }

        if (ImGui::BeginCombo("Scene", m_storyJson->GetScene(), 0)) // The second parameter is the label previewed before opening the combo.
        {

            for (int i = 0; i < sceneTable->GetSize(); i++)
            {
                bool is_selected = strcmp(m_storyJson->GetScene(), sceneTable->GetRow(i)->Get(0)) == 0;
                if (ImGui::Selectable(sceneTable->GetRow(i)->Get(0), is_selected))
                    m_storyJson->SetScene(sceneTable->GetRow(i)->Get(0));
                if (is_selected)
                    ImGui::SetItemDefaultFocus();   // Set the initial focus when opening the combo (scrolling + for keyboard navigation support in the upcoming navigation branch)


            }

            ImGui::EndCombo();
        }

        static char* name = nullptr;
        static char* content = nullptr;
        static char* label = nullptr;
        static char* jump = nullptr;
        static char* jumpContent = nullptr;

        char order[8] = "";
        char thumbnail[MAX_CONTENT * 2] = "";

        char currentId[NUM_OF_ID_PART][MAX_ID_COUNT];
        memset(currentId, '\0', NUM_OF_ID_PART*MAX_ID_COUNT);

        ImGui::LabelText("label", "Value");
        if (m_storyJson != nullptr) {
            for (int i = 0; i < m_storyJson->Size(); i++)
            {

                sprintf(order, "%d", i);

                StoryNode* node = m_storyJson->GetNode(i);
                switch (node->m_nodeType) {
                case NodeType::Word:
                {
                    char nameConstant[16] = "Name";
                    char contentConstant[16] = "Content";
                    strcat(nameConstant, order);
                    strcat(contentConstant, order);

                    StoryWord* word = static_cast<StoryWord*>(node);
                    name = word->m_name;
                    content = word->m_content;

                    strcpy(thumbnail, order);
                    strcat(thumbnail, "_Word_");
                    strcat(thumbnail, name);
                    strcat(thumbnail, ": ");
                    strcat(thumbnail, content);

                    if (ImGui::TreeNode((void*)(intptr_t)i, thumbnail))
                    {

                        AddButton(i);

                        ImGui::InputTextWithHint(nameConstant, "Enter name here", name, MAX_NAME);
                        ImGui::InputTextWithHint(contentConstant, "Enter Content here", content, MAX_CONTENT, ImGuiInputTextFlags_CallbackAlways, WordContentCallback);

                        ImGui::TreePop();
                    }


                    break;
                }

                case NodeType::Jump:
                {


                    char jumpConstant[16] = "Jump";
                    char contentConstant[16] = "JumpContent";
                    char chapterConstant[16] = "Chapter";
                    char sceneConstant[16] = "Scene";
                    char idTitileConstant[16] = "Id Title";
                    char idCountConstant[16] = "Id Count";
                    strcat(jumpConstant, order);
                    strcat(contentConstant, order);
                    strcat(chapterConstant, order);
                    strcat(sceneConstant, order);
                    strcat(idTitileConstant, order);
                    strcat(idCountConstant, order);

                    StoryJump* pJump = static_cast<StoryJump*>(node);
                    jump = pJump->m_jumpId;
                    jumpContent = pJump->m_jumpContent;

                    strcpy(thumbnail, order);
                    strcat(thumbnail, "_Jump_");
                    strcat(thumbnail, jump);

                    if (ImGui::TreeNode((void*)(intptr_t)i, thumbnail))
                    {

                        AddButton(i);
                        ImGui::Text("Jump Id: %s", jump);

                        IdOperator::ParseStringId(jump, currentId);

                        if (ImGui::BeginCombo(chapterConstant, currentId[(int)ID_PART::CHAPTER], 0)) // The second parameter is the label previewed before opening the combo.
                        {

                            for (int i = 0; i < chapterTable->GetSize(); i++)
                            {
                                bool is_selected = strcmp(currentId[(int)ID_PART::CHAPTER], chapterTable->GetRow(i)->Get(0)) == 0;
                                if (ImGui::Selectable(chapterTable->GetRow(i)->Get(0), is_selected)) {
                                    strcpy(currentId[(int)ID_PART::CHAPTER], chapterTable->GetRow(i)->Get(0));

                                }
                                if (is_selected)
                                    ImGui::SetItemDefaultFocus();   // Set the initial focus when opening the combo (scrolling + for keyboard navigation support in the upcoming navigation branch)

                            }

                            ImGui::EndCombo();
                        }

                        if (ImGui::BeginCombo(sceneConstant, currentId[(int)ID_PART::SCENE], 0)) // The second parameter is the label previewed before opening the combo.
                        {

                            for (int i = 0; i < sceneTable->GetSize(); i++)
                            {
                                bool is_selected = strcmp(currentId[(int)ID_PART::SCENE], sceneTable->GetRow(i)->Get(0)) == 0;
                                if (ImGui::Selectable(sceneTable->GetRow(i)->Get(0), is_selected)) {
                                    strcpy(currentId[(int)ID_PART::SCENE], sceneTable->GetRow(i)->Get(0));

                                }
                                if (is_selected)
                                    ImGui::SetItemDefaultFocus();   // Set the initial focus when opening the combo (scrolling + for keyboard navigation support in the upcoming navigation branch)

                            }

                            ImGui::EndCombo();
                        }

                        ImGui::InputText(idTitileConstant, currentId[(int)ID_PART::TITLE], MAX_ID_TITLE, ImGuiInputTextFlags_CharsNoBlank);

                        ImGui::InputText(idCountConstant, currentId[(int)ID_PART::COUNT], MAX_ID_COUNT, ImGuiInputTextFlags_CharsDecimal);

                        IdOperator::CombineStringId(pJump->m_jumpId, currentId);


                        ImGui::TextColored(ImVec4(1.0f, 0.0f, 1.0f, 1.0f), jump);
                        ImGui::InputTextWithHint(contentConstant, "Enter jump Content here", jumpContent, MAX_ID);

                        ImGui::TreePop();
                    }
                    break;
                }

                case NodeType::Label:
                {


                    char LabelConstant[16] = "Label";
                    char chapterConstant[16] = "Chapter";
                    char sceneConstant[16] = "Scene";
                    char idTitileConstant[16] = "Id Title";
                    char idCountConstant[16] = "Id Count";

                    strcat(LabelConstant, order);

                    strcat(chapterConstant, order);
                    strcat(sceneConstant, order);
                    strcat(idTitileConstant, order);
                    strcat(idCountConstant, order);

                    StoryLabel *pLabel = static_cast<StoryLabel*>(node);
                    label = pLabel->m_labelId;

                    strcpy(thumbnail, order);
                    strcat(thumbnail, "_Label_");
                    strcat(thumbnail, label);

                    if (ImGui::TreeNode((void*)(intptr_t)i, thumbnail))
                    {
                        AddButton(i);
                        ImGui::Text("Jump Id: %s", label);

                        IdOperator::ParseStringId(label, currentId);

                        if (ImGui::BeginCombo(chapterConstant, currentId[(int)ID_PART::CHAPTER], 0)) // The second parameter is the label previewed before opening the combo.
                        {

                            for (int i = 0; i < chapterTable->GetSize(); i++)
                            {
                                bool is_selected = strcmp(currentId[(int)ID_PART::CHAPTER], chapterTable->GetRow(i)->Get(0)) == 0;
                                if (ImGui::Selectable(chapterTable->GetRow(i)->Get(0), is_selected)) {
                                    strcpy(currentId[(int)ID_PART::CHAPTER], chapterTable->GetRow(i)->Get(0));

                                }
                                if (is_selected)
                                    ImGui::SetItemDefaultFocus();   // Set the initial focus when opening the combo (scrolling + for keyboard navigation support in the upcoming navigation branch)

                            }

                            ImGui::EndCombo();
                        }

                        if (ImGui::BeginCombo(sceneConstant, currentId[(int)ID_PART::SCENE], 0)) // The second parameter is the label previewed before opening the combo.
                        {

                            for (int i = 0; i < sceneTable->GetSize(); i++)
                            {
                                bool is_selected = strcmp(currentId[(int)ID_PART::SCENE], sceneTable->GetRow(i)->Get(0)) == 0;
                                if (ImGui::Selectable(sceneTable->GetRow(i)->Get(0), is_selected)) {
                                    strcpy(currentId[(int)ID_PART::SCENE], sceneTable->GetRow(i)->Get(0));

                                }
                                if (is_selected)
                                    ImGui::SetItemDefaultFocus();   // Set the initial focus when opening the combo (scrolling + for keyboard navigation support in the upcoming navigation branch)
                            }

                            ImGui::EndCombo();
                        }

                        ImGui::InputText(idTitileConstant, currentId[(int)ID_PART::TITLE], MAX_ID_TITLE, ImGuiInputTextFlags_CharsNoBlank);

                        ImGui::InputText(idCountConstant, currentId[(int)ID_PART::COUNT], MAX_ID_COUNT, ImGuiInputTextFlags_CharsDecimal);

                        IdOperator::CombineStringId(pLabel->m_labelId, currentId);

                        //ImGui::InputTextWithHint(LabelConstant, "Enter label ID here", label, MAX_NAME);
                        ImGui::InputTextWithHint(LabelConstant, "Enter label ID here", label, MAX_ID);
                        ImGui::TextColored(ImVec4(1.0f, 0.0f, 1.0f, 1.0f), label);
                        ImGui::TreePop();
                    }
                    break;
                }

                default:
                    break;
                }

            }
        }

        if (ImGui::Button("Add new story")) {

            if (m_storyJson != nullptr)
            {
                m_storyJson->AddWord("", "");

                AddNotification("Add New Story Word");
            }
        }

        ImGui::SameLine();

        if (ImGui::Button("Add new label")) {

            if (m_storyJson != nullptr)
            {
                m_storyJson->AddLabel("");

                AddNotification("Add New Label");
            }
        }
        ImGui::SameLine();

        if (ImGui::Button("Add new jump")) {

            if (m_storyJson != nullptr)
            {
                m_storyJson->AddJump("", "");

                AddNotification("Add New Jump");
            }
        }


        //Update Auto Save
        StoryTimer<HeavenGateWindowStoryEditor>::Update();
    }

    void HeavenGateWindowStoryEditor::UpdateMenu()
    {

        if (ImGui::MenuItem("New")) {
            m_inputFileNamePopup->OpenWindow();
        }
        if (ImGui::MenuItem("Open", "Ctrl+O")) {
            m_selectStoryWindow->OpenWindow();
        }
        if (ImGui::BeginMenu("Open Recent"))
        {
            ImGui::MenuItem("fish_hat.c");
            ImGui::MenuItem("fish_hat.inl");
            ImGui::MenuItem("fish_hat.h");
            if (ImGui::BeginMenu("More.."))
            {
                ImGui::MenuItem("Hello");
                ImGui::MenuItem("Sailor");
                if (ImGui::BeginMenu("Recurse.."))
                {

                    ImGui::EndMenu();
                }
                ImGui::EndMenu();
            }
            ImGui::EndMenu();
        }
        if (ImGui::MenuItem("Save", "Ctrl+S")) {

            bool result = StoryFileManager::Instance().SaveStoryFile(m_storyJson);

            if (result)
            {
                AddNotification("Successful to Save File");
            }
            else
            {
                AddNotification("Failed to Save File");
            }

        }
        if (ImGui::MenuItem("Export", "Ctrl+E")) {

            bool result = StoryFileManager::Instance().ExportStoryFile(m_storyJson);

            if (result)
            {
                AddNotification("Successful to Export File");
            }
            else
            {
                AddNotification("Failed to Export File");
            }

        }
        if (ImGui::MenuItem("Save As..")) {

            AddNotification("This function is not finish yet.");
        }




        ImGui::Separator();
        if (ImGui::BeginMenu("Options"))
        {
            static bool enabled = true;
            ImGui::MenuItem("Enabled", "", &enabled);
            ImGui::BeginChild("child", ImVec2(0, 60), true);
            for (int i = 0; i < 10; i++)
                ImGui::Text("Scrolling Text %d", i);
            ImGui::EndChild();
            static float f = 0.5f;
            static int n = 0;
            static bool b = true;
            ImGui::SliderFloat("Value", &f, 0.0f, 1.0f);
            ImGui::InputFloat("Input", &f, 0.1f);
            ImGui::Combo("Combo", &n, "Yes\0No\0Maybe\0\0");
            ImGui::Checkbox("Check", &b);
            ImGui::EndMenu();
        }
        if (ImGui::BeginMenu("Colors"))
        {
            float sz = ImGui::GetTextLineHeight();
            for (int i = 0; i < ImGuiCol_COUNT; i++)
            {
                const char* name = ImGui::GetStyleColorName((ImGuiCol)i);
                ImVec2 p = ImGui::GetCursorScreenPos();
                ImGui::GetWindowDrawList()->AddRectFilled(p, ImVec2(p.x + sz, p.y + sz), ImGui::GetColorU32((ImGuiCol)i));
                ImGui::Dummy(ImVec2(sz, sz));
                ImGui::SameLine();
                ImGui::MenuItem(name);
            }
            ImGui::EndMenu();
        }
        if (ImGui::BeginMenu("Disabled", false)) // Disabled
        {
            IM_ASSERT(0);
        }
        if (ImGui::MenuItem("Checked", NULL, true)) {}
        if (ImGui::MenuItem("Quit", "Alt+F4")) {}
    }


    void HeavenGateWindowStoryEditor::AddButton(int index)
    {
        if (ImGui::TreeNode("Story Operator"))
        {
            if (ImGui::Button("Insert new story")) {

                if (m_storyJson != nullptr)
                {
                    m_storyJson->InsertWord("", "", index);

                    AddNotification("Insert New Story Word");
                }
            }

            ImGui::SameLine();
            if (ImGui::Button("Insert new label")) {

                if (m_storyJson != nullptr)
                {
                    m_storyJson->InsertLabel("", index);

                    AddNotification("Insert New Label");

                }
            }
            ImGui::SameLine();

            if (ImGui::Button("Insert new jump")) {

                if (m_storyJson != nullptr)
                {
                    m_storyJson->InsertJump("", "", index);

                    AddNotification("Insert New Jump");

                }
            }

            ImGui::SameLine();

            if (ImGui::Button("Move Up")) {

                if (m_storyJson != nullptr)
                {
                    m_storyJson->Swap(index, index - 1);

                    AddNotification("Current Node Move Up");
                }
            }
            ImGui::SameLine();

            if (ImGui::Button("Move Down")) {

                if (m_storyJson != nullptr)
                {
                    m_storyJson->Swap(index, index + 1);

                    AddNotification("Current Node Move Down");

                }
            }

            if (ImGui::Button("Delete it")) {
                if (m_storyJson != nullptr) {
                    m_storyJson->Remove(index);

                    AddNotification("The node was deleted");

                }
            }

            ImGui::TreePop();
        }
    }




    void HeavenGateWindowStoryEditor::AddNotification(const char * const notification)
    {
        strcpy(m_notification, notification);
    }

    int HeavenGateWindowStoryEditor::WordContentCallback(ImGuiInputTextCallbackData * data)
    {

        TableType currentState;
        deque<TableType> editorState;
        bool isReadCloseLabel;

        isReadCloseLabel = false;
        currentState = TableType::None;

        StoryWord compiledWord;
        strcpy(compiledWord.m_content, data->Buf);
        vector<StoryJsonContentCompiler::Token*>tokens = StoryJsonContentCompiler::Instance().CompileToTokens(&compiledWord);

        //Tmp value
        ImVec4 color(1.0f, 1.0f, 1.0f, 1.0f);

        char tmpTip[MAX_COLUMNS_CONTENT_LENGTH];
        memset(tmpTip, '\0', MAX_COLUMNS_CONTENT_LENGTH);

        char tmpExhibit[MAX_COLUMNS_CONTENT_LENGTH];
        memset(tmpExhibit, '\0', MAX_COLUMNS_CONTENT_LENGTH);

        bool isExhibit = false;

        //Color
        ImVec4 colorBlue = ImVec4(0.0f, 0.0f, 1.0f, 1.0f);

        ImGui::Text("Preview:");

        for (auto iter = tokens.cbegin(); iter != tokens.end(); iter++)
        {
            if ((*iter)->m_tokeType == StoryJsonContentCompiler::TokenType::TokenContent)
            {
                ImGui::TextColored(color, (*iter)->m_content);
                ImGui::SameLine(0, 0);
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
                        break;
                    case HeavenGateEditor::TableType::Color:
                        color = ImVec4(1.0f, 1.0f, 1.0f, 1.0f);
                        break;
                    case HeavenGateEditor::TableType::Tips:
                        ImGui::TextColored(color, "[Show Tip Here: %s]", tmpTip);
                        ImGui::SameLine(0, 0);
                        break;
                    case HeavenGateEditor::TableType::Paint_Move:
                        break;
                    case HeavenGateEditor::TableType::Exhibit:
                        if (isExhibit == true) {
                            ImGui::TextColored(colorBlue, "[Show Exhibit Here: %s]", tmpExhibit);
                            isExhibit = false;
                        }
                        break;
                    case TableType::Pause:
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
                    if (strcmp((*iter)->m_content, exhibitTableString[(int)ExhibitTableLayout::Type]) == 0) {
                        editorState.push_back(TableType::Exhibit);
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
                    const StoryTable<EXHIBIT_COLUMN>* const exhibitTable = StoryTableManager::Instance().GetExhibitTable();

                    for (int i = 0; i < exhibitTable->GetSize(); i++)
                    {
                        const StoryRow<EXHIBIT_COLUMN>* const row = exhibitTable->GetRow(i);
                        if (strcmp(row->Get(0), (*iter)->m_content) == 0)
                        {
                            strcpy(tmpExhibit, row->Get(0));
                        }
                    }
                    isExhibit = true;
                    break;
                }
                case HeavenGateEditor::TableType::Tips: {
                    const StoryTable<TIP_MAX_COLUMN>* const tipTable = StoryTableManager::Instance().GetTipTable();

                    for (int i = 0; i < tipTable->GetSize(); i++)
                    {
                        const StoryRow<TIP_MAX_COLUMN>* const row = tipTable->GetRow(i);
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

        ImGui::Text("");
        ImGui::Separator();
        return 0;
    }




    void HeavenGateWindowStoryEditor::ShowStoryWord(StoryWord* word, int index)
    {
        static char* name = nullptr;
        static char* content = nullptr;
        char order[8] = "";
        char thumbnail[MAX_CONTENT * 2] = "";
        char nameConstant[16] = "Name ";
        char contentConstant[16] = "Content ";


        sprintf(order, "%d", index);
        strcat(nameConstant, order);
        strcat(contentConstant, order);

        //StoryWord* word = static_cast<StoryWord*>(node);
        name = word->m_name;
        content = word->m_content;

        strcpy(thumbnail, order);
        strcat(thumbnail, "_Word_");
        strcat(thumbnail, name);
        strcat(thumbnail, ": ");
        strcat(thumbnail, content);

        if (ImGui::TreeNode((void*)(intptr_t)index, thumbnail))
        {

            AddButton(index);

            ImGui::InputTextWithHint(nameConstant, "Enter name here", name, MAX_NAME);
            ImGui::InputTextWithHint(contentConstant, "Enter Content here", content, MAX_CONTENT, ImGuiInputTextFlags_CallbackAlways, WordContentCallback);


            ImGui::TreePop();
        }

    }

    void HeavenGateWindowStoryEditor::AutoSaveCallback()
    {
        if (m_storyJson != nullptr)
        {
            bool result = StoryFileManager::Instance().AutoSaveStoryFile(m_storyJson);

            if (result)
            {
                AddNotification("Successful to Auto Save File");
            }
            else
            {
                AddNotification("Failed to Auto Save File");
            }

        }
    }

    //void HeavenGateWindowStoryEditor::ShowEditorWindow(bool* isOpenPoint) {


    //    m_selectStoryWindow->ShowSelectStoryWindow();
    //    m_selectStoryWindow->GetStoryPointerWindow(&m_story, &m_isSavedFileInCurrentWindow);

    //    //if (m_selectStoryWindow->IsLoadedSotry()) {
    //    //    if (m_story == nullptr) {
    //    //        m_selectStoryWindow->GetStoryPointer(&m_story);
    //    //        m_selectStoryWindow->GiveUpLoadedStory();
    //    //        m_isSavedFile = true;
    //    //    }
    //    //    else
    //    //    {
    //    //        ImGui::OpenPopup("Have Unsaved Content");

    //    //        if (ImGui::BeginPopupModal("Have Unsaved Content", NULL, ImGuiWindowFlags_AlwaysAutoResize))
    //    //        {
    //    //            ImGui::Text("You open a new file, but now workspace already have changes.\n Do you want to abandon changes to open new file or keep them?\n\n");
    //    //            ImGui::Separator();

    //    //            if (ImGui::Button("Keep Changes", ImVec2(120, 0))) {

    //    //                m_selectStoryWindow->GiveUpLoadedStory();
    //    //                ImGui::CloseCurrentPopup();

    //    //            }
    //    //            ImGui::SetItemDefaultFocus();
    //    //            ImGui::SameLine();
    //    //            if (ImGui::Button("Give Up Changes", ImVec2(120, 0))) {

    //    //                delete m_story;
    //    //                m_story = nullptr;

    //    //                m_selectStoryWindow->GetStoryPointer(&m_story);
    //    //                m_selectStoryWindow->GiveUpLoadedStory();

    //    //                m_isSavedFile = true;
    //    //                ImGui::CloseCurrentPopup();
    //    //            }
    //    //            ImGui::EndPopup();
    //    //        }
    //    //    }

    //    //}

    //    // Demonstrate the various window flags. Typically you would just use the default!
    //    static bool no_titlebar = false;
    //    static bool no_scrollbar = false;
    //    static bool no_menu = false;
    //    static bool no_move = false;
    //    static bool no_resize = false;
    //    static bool no_collapse = false;
    //    static bool no_close = false;
    //    static bool no_nav = false;
    //    static bool no_background = false;
    //    static bool no_bring_to_front = false;

    //    ImGuiWindowFlags window_flags = 0;
    //    if (no_titlebar)        window_flags |= ImGuiWindowFlags_NoTitleBar;
    //    if (no_scrollbar)       window_flags |= ImGuiWindowFlags_NoScrollbar;
    //    if (!no_menu)           window_flags |= ImGuiWindowFlags_MenuBar;
    //    if (no_move)            window_flags |= ImGuiWindowFlags_NoMove;
    //    if (no_resize)          window_flags |= ImGuiWindowFlags_NoResize;
    //    if (no_collapse)        window_flags |= ImGuiWindowFlags_NoCollapse;
    //    if (no_nav)             window_flags |= ImGuiWindowFlags_NoNav;
    //    if (no_background)      window_flags |= ImGuiWindowFlags_NoBackground;
    //    if (no_bring_to_front)  window_flags |= ImGuiWindowFlags_NoBringToFrontOnFocus;
    //    if (no_close)           isOpenPoint = NULL; // Don't pass our bool* to Begin


    //  // We specify a default position/size in case there's no data in the .ini file. Typically this isn't required! We only do it to make the Demo applications a little more welcoming.
    //    ImGui::SetNextWindowPos(ImVec2(650, 20), ImGuiCond_FirstUseEver);
    //    ImGui::SetNextWindowSize(ImVec2(550, 680), ImGuiCond_FirstUseEver);

    //    // Main body of the Demo window starts here.
    //    if (!ImGui::Begin("Heaven Gate", isOpenPoint, window_flags))
    //    {
    //        // Early out if the window is collapsed, as an optimization.
    //        ImGui::End();
    //        return;
    //    }

    //    // Most "big" widgets share a common width settings by default.
    //    //ImGui::PushItemWidth(ImGui::GetWindowWidth() * 0.65f);    // Use 2/3 of the space for widgets and 1/3 for labels (default)
    //    ImGui::PushItemWidth(ImGui::GetFontSize() * -12);           // Use fixed width for labels (by passing a negative value), the rest goes to widgets. We choose a width proportional to our font size.

    //    // Menu Bar
    //    if (ImGui::BeginMenuBar())
    //    {
    //        if (ImGui::BeginMenu("Menu"))
    //        {
    //            ShowEditorMenuFile();
    //            ImGui::EndMenu();
    //        }

    //        ImGui::EndMenuBar();
    //    }

    //    //For save as new file
    //    m_fileManagerWindow->Update();
    //    if (m_fileManagerWindow->IsExistNewFilePath())
    //    {
    //        if (!m_fileManagerWindow->SaveStoryFile(m_story)) {
    //            return;
    //        }
    //        const char* filePath = m_fileManagerWindow->GetNewFilePath();
    //        m_story->SetFullPath(filePath);
    //        m_isSavedFileInCurrentWindow = true;
    //        m_fileManagerWindow->Initialize();
    //    }

    //    ImGui::Text("Heaven Gate says hello. (%s)", IMGUI_VERSION);
    //    ImGui::Spacing();
    //    if (m_isSavedFileInCurrentWindow == true &&
    //        m_story != nullptr &&
    //        m_story->IsExistFullPath() == true) {
    //        ImGui::Text("Current story path: %s", m_story->GetFullPath());

    //    }


    //    //        if (!isSavedFile && m_story == nullptr)
    //    //        {
    //    //            m_selectStoryWindow->GetStoryPointer(m_story);
    //    //            if (m_story == nullptr)
    //    //            {
    //    //                //Create a new story
    //    //                m_story = new StoryJson();
    //    //
    //    //            }
    //    //
    //    //        }
    //    static ImGuiInputTextFlags flags = ImGuiInputTextFlags_AllowTabInput;
    //    ImGui::CheckboxFlags("ImGuiInputTextFlags_ReadOnly", (unsigned int*)&flags, ImGuiInputTextFlags_ReadOnly);
    //    ImGui::CheckboxFlags("ImGuiInputTextFlags_AllowTabInput", (unsigned int*)&flags, ImGuiInputTextFlags_AllowTabInput);
    //    ImGui::CheckboxFlags("ImGuiInputTextFlags_CtrlEnterForNewLine", (unsigned int*)&flags, ImGuiInputTextFlags_CtrlEnterForNewLine);
    //    static char* name = nullptr;
    //    static char* content = nullptr;
    //    static char* label = nullptr;
    //    static char* jump = nullptr;

    //    char order[8] = "";
    //    ImGui::LabelText("label", "Value");
    //    if (m_story != nullptr) {
    //        for (int i = 0; i < m_story->Size(); i++)
    //        {

    //  
    //            
    //            sprintf(order, "%d", i);
    //     
    //            StoryNode* node = m_story->GetNode(i);
    //            switch (node->m_nodeType) {
    //            case NodeType::Word:
    //            {
    //                char nameConstant[16] = "Name";
    //                char contentConstant[16] = "Content";
    //                strcat(nameConstant, order);
    //                strcat(contentConstant, order);

    //                StoryWord* word = static_cast<StoryWord*>(node);
    //                name = word->m_name;
    //                content = word->m_content;


    //                ImGui::InputTextWithHint(nameConstant, "Enter name here", name, MAX_NAME);
    //                ImGui::InputTextWithHint(contentConstant, "Enter Content here", content, MAX_CONTENT);

    //                //Multiline Version
    //                /*
    //                ImGui::InputTextMultiline(nameConstant, name, MAX_NAME, ImVec2(-FLT_MIN, ImGui::GetTextLineHeight() * 2), flags);
    //                ImGui::InputTextMultiline(contentConstant, content, MAX_CONTENT, ImVec2(-FLT_MIN, ImGui::GetTextLineHeight() * 6), flags);
    //*/
    //                break;
    //            }

    //            case NodeType::Jump:
    //            {
    //                char jumpConstant[16] = "Jump";
    //                strcat(jumpConstant, order);

    //                StoryJump* pJump = static_cast<StoryJump*>(node);
    //                jump = pJump->m_jumpId;

    //                ImGui::InputTextWithHint(jumpConstant, "Enter jump ID here", jump, MAX_NAME);
    //                break;
    //            }

    //            case NodeType::Label:
    //            {
    //                char LabelConstant[16] = "Label";
    //                strcat(LabelConstant, order);

    //                StoryLabel *pLabel = static_cast<StoryLabel*>(node);
    //                label = pLabel->m_labelId;

    //                ImGui::InputTextWithHint(LabelConstant, "Enter label ID here", label, MAX_NAME);
    //                break;
    //            }

    //            default:
    //                break;
    //            }

    //        }
    //    }

    //    if (ImGui::Button("Add new story")) {
    //        //If story not exist
    //        if (m_story == nullptr)
    //        {
    //            m_story = new StoryJson;
    //        }
    //        //If already exist story
    //        if (m_story != nullptr)
    //        {
    //            m_story->AddWord("", "");
    //        }
    //    }

    //    ImGui::SameLine();

    //    if (ImGui::Button("Add new label")) {
    //        //If story not exist
    //        if (m_story == nullptr)
    //        {
    //            m_story = new StoryJson;
    //        }
    //        //If already exist story
    //        if (m_story != nullptr)
    //        {
    //            m_story->AddLabel("");
    //        }
    //    }
    //    ImGui::SameLine();

    //    if (ImGui::Button("Add new jump")) {
    //        //If story not exist
    //        if (m_story == nullptr)
    //        {
    //            m_story = new StoryJson;
    //        }
    //        //If already exist story
    //        if (m_story != nullptr)
    //        {
    //            m_story->AddJump("");
    //        }
    //    }
    //    //
    //    //        for (int i = 0; i < currentStory.size(); i++)
    //    //        {
    //    //
    //    //        }

    //            // End of ShowDemoWindow()


    //    ImGui::End();

    //}

    //// Note that shortcuts are currently provided for display only (future version will add flags to BeginMenu to process shortcuts)
    //void HeavenGateEditor::ShowEditorMenuFile()
    //{

    //    //   ImGui::MenuItem("(dummy menu)", NULL, false, false);
    //    if (ImGui::MenuItem("New")) {

    //    }
    //    if (ImGui::MenuItem("Open", "Ctrl+O")) {
    //        m_selectStoryWindow->OpenWindow();
    //    }
    //    if (ImGui::BeginMenu("Open Recent"))
    //    {
    //        ImGui::MenuItem("fish_hat.c");
    //        ImGui::MenuItem("fish_hat.inl");
    //        ImGui::MenuItem("fish_hat.h");
    //        if (ImGui::BeginMenu("More.."))
    //        {
    //            ImGui::MenuItem("Hello");
    //            ImGui::MenuItem("Sailor");
    //            if (ImGui::BeginMenu("Recurse.."))
    //            {
    //                ShowEditorMenuFile();
    //                ImGui::EndMenu();
    //            }
    //            ImGui::EndMenu();
    //        }
    //        ImGui::EndMenu();
    //    }
    //    if (ImGui::MenuItem("Save", "Ctrl+S")) {


    //        if (m_isSavedFileInCurrentWindow) {

    //            m_storyFileManager->SaveStoryFile(m_story);

    //            //m_fileManagerWindow->SetNewFilePath(m_story->GetFullPath());
    //            //if (!m_fileManagerWindow->SaveStoryFile(m_story)) {
    //            //    return;
    //            //}
    //            //m_fileManagerWindow->Initialize();

    //        }
    //        else {

    //            m_fileManagerWindow->OpenWindow();

    //        }
    //    }
    //    if (ImGui::MenuItem("Save As..")) {


    //    }




    //    ImGui::Separator();
    //    if (ImGui::BeginMenu("Options"))
    //    {
    //        static bool enabled = true;
    //        ImGui::MenuItem("Enabled", "", &enabled);
    //        ImGui::BeginChild("child", ImVec2(0, 60), true);
    //        for (int i = 0; i < 10; i++)
    //            ImGui::Text("Scrolling Text %d", i);
    //        ImGui::EndChild();
    //        static float f = 0.5f;
    //        static int n = 0;
    //        static bool b = true;
    //        ImGui::SliderFloat("Value", &f, 0.0f, 1.0f);
    //        ImGui::InputFloat("Input", &f, 0.1f);
    //        ImGui::Combo("Combo", &n, "Yes\0No\0Maybe\0\0");
    //        ImGui::Checkbox("Check", &b);
    //        ImGui::EndMenu();
    //    }
    //    if (ImGui::BeginMenu("Colors"))
    //    {
    //        float sz = ImGui::GetTextLineHeight();
    //        for (int i = 0; i < ImGuiCol_COUNT; i++)
    //        {
    //            const char* name = ImGui::GetStyleColorName((ImGuiCol)i);
    //            ImVec2 p = ImGui::GetCursorScreenPos();
    //            ImGui::GetWindowDrawList()->AddRectFilled(p, ImVec2(p.x + sz, p.y + sz), ImGui::GetColorU32((ImGuiCol)i));
    //            ImGui::Dummy(ImVec2(sz, sz));
    //            ImGui::SameLine();
    //            ImGui::MenuItem(name);
    //        }
    //        ImGui::EndMenu();
    //    }
    //    if (ImGui::BeginMenu("Disabled", false)) // Disabled
    //    {
    //        IM_ASSERT(0);
    //    }
    //    if (ImGui::MenuItem("Checked", NULL, true)) {}
    //    if (ImGui::MenuItem("Quit", "Alt+F4")) {}
    //}





}

