

#include "HeavenGateWindowStoryEditor.h"
#include "CharacterUtility.h"

#include "HeavenGateWindowSelectStory.h"
#include "HeavenGatePopupInputFileName.h"
#include "HeavenGatePopupMessageBox.h"
#include "HeavenGateWindowPreview.h"
#include "StoryTimer.h"

#include "StoryJsonManager.h"
#include "StoryJson.h"
#include "StoryJsonWordNode.h"
#include "StoryJsonLabelNode.h"
#include "StoryJsonJumpNode.h"
#include "StoryJsonExhibitNode.h"
#include "StoryJsonEventNode.h"
#include "StoryJsonEndNode.h"
#include "StoryColor.h"


#include "StoryTable.h"
#include "StoryFileManager.h"
#include "StoryTableManager.h"

#include <iostream>

#include <stdio.h>
#include <assert.h>  


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
        m_selectStoryWindow = nullptr;
        m_selectStoryWindow = new HeavenGateWindowSelectStory();
        m_selectStoryWindow->Initialize();

        m_inputFileNamePopup = nullptr;
        m_inputFileNamePopup = new HeavenGatePopupInputFileName(this);
        m_inputFileNamePopup->Initialize();

        m_messageBoxPopup = nullptr;
        m_messageBoxPopup = new HeavenGatePopupMessageBox();
        m_messageBoxPopup->Initialize();

        m_previewWindow = nullptr;
        m_previewWindow = new HeavenGateWindowPreview();
        m_previewWindow->Initialize();

        //Default open select story window
        bool* selectStoryWindowHandle = m_selectStoryWindow->GetHandle();
        *selectStoryWindowHandle = true;

        bool* previewWindowHandle = m_previewWindow->GetHandle();
        *previewWindowHandle = true;

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

        if (m_messageBoxPopup != nullptr)
        {
            m_messageBoxPopup->Shutdown();
            delete m_messageBoxPopup;
        }
        m_messageBoxPopup = nullptr;

        if (m_previewWindow != nullptr) {
            m_previewWindow->Shutdown();
            delete m_previewWindow;
        }
        m_previewWindow = nullptr;

        m_storyJson = nullptr;

        memset(m_notification, '\0', sizeof(m_notification));
    }

    void HeavenGateWindowStoryEditor::UpdateMainWindow()
    {
        m_selectStoryWindow->Update();

        //For save as new file
        m_inputFileNamePopup->Update();

        m_messageBoxPopup->Update();

        m_previewWindow->Update();

        if (m_storyJson == nullptr)
        {
            m_storyJson = StoryJsonManager::Instance().GetStoryJson();
            if (m_storyJson == nullptr)  return;
        }

        ImGui::Text("Heaven Gate. (%s)\nImgui version. (%s)", EDITOR_VERSION, IMGUI_VERSION);
        ImGui::TextColored(ImVec4(1.0f, 0.0f, 1.0f, 1.0f), m_notification);
        ImGui::Spacing();
        if (m_storyJson != nullptr &&
            m_storyJson->IsExistFullPath() == true) {
            char tmpFullPath [MAX_FOLDER_PATH];
            strcpy(tmpFullPath, m_storyJson->GetFullPath());
            ImGui::InputText("Current story path: %s",tmpFullPath, MAX_FOLDER_PATH, ImGuiInputTextFlags_ReadOnly);
        }

        static char* name = nullptr;
        static char* content = nullptr;
        static char* label = nullptr;
        static char* exhibitID = nullptr;
        static char* exhibitPrefix = nullptr;
        static char* eventName = nullptr;
        static char* jump = nullptr;
        static char* jumpContent = nullptr;

        char order[8] = "";
        char thumbnail[MAX_CONTENT * 2] = "";

        assert(NUM_OF_ID_PART * MAX_ID_COUNT + NUM_OF_ID_PART == MAX_ID);
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
                    StoryWord* word = static_cast<StoryWord*>(node);
                    ShowStoryWord(word, i);
                    break;
                }

                case NodeType::Jump:
                {
                    char jumpConstant[16] = "Jump";
                    char contentConstant[16] = "JumpContent";
                    //char chapterConstant[16] = "Chapter";
                    //char sceneConstant[16] = "Scene";
                    char idTitileConstant[16] = "Id Title";
                    char idCountConstant[16] = "Id Count";
                    strcat(jumpConstant, order);
                    strcat(contentConstant, order);
                    //strcat(chapterConstant, order);
                    //strcat(sceneConstant, order);
                    strcat(idTitileConstant, order);
                    strcat(idCountConstant, order);

                    StoryJump* pJump = static_cast<StoryJump*>(node);
                    jump = pJump->m_jumpId;
                    jumpContent = pJump->m_jumpContent;

                    strcpy(thumbnail, order);
                    strcat(thumbnail, "_Jump: ");
                    strcat(thumbnail, jump);

                    if (ImGui::TreeNode((void*)(intptr_t)i, thumbnail))
                    {

                        AddButton(i);
                        ImGui::Text("Jump Id: %s", jump);

                        IdOperator::ParseStringId<'_', MAX_ID_PART, NUM_OF_ID_PART>(jump, currentId);
                        if (!IsNum(currentId[(int)ID_PART::COUNT])) {
                            strcpy(currentId[(int)ID_PART::COUNT], "0");
                        }

                        ImGui::InputText(idTitileConstant, currentId[(int)ID_PART::TITLE], MAX_ID_TITLE, ImGuiInputTextFlags_CharsNoBlank);
                        ImGui::InputText(idCountConstant, currentId[(int)ID_PART::COUNT], MAX_ID_COUNT, ImGuiInputTextFlags_CharsDecimal);

                        IdOperator::CombineStringId<'_', MAX_ID_PART, NUM_OF_ID_PART>(pJump->m_jumpId, currentId);


                        ImGui::TextColored(ImVec4(1.0f, 0.0f, 1.0f, 1.0f), jump);
                        ImGui::InputTextWithHint(contentConstant, "Enter jump Content here", jumpContent, MAX_ID);

                        ImGui::TreePop();
                    }
                    break;
                }

                case NodeType::Label:
                {
                    char LabelConstant[16] = "Label";
                    //char chapterConstant[16] = "Chapter";
                    //char sceneConstant[16] = "Scene";
                    char idTitileConstant[16] = "Id Title";
                    char idCountConstant[16] = "Id Count";

                    strcat(LabelConstant, order);

                    //strcat(chapterConstant, order);
                    //strcat(sceneConstant, order);
                    strcat(idTitileConstant, order);
                    strcat(idCountConstant, order);

                    StoryLabel *pLabel = static_cast<StoryLabel*>(node);
                    label = pLabel->m_labelId;

                    strcpy(thumbnail, order);
                    strcat(thumbnail, "_Label: ");
                    strcat(thumbnail, label);

                    if (ImGui::TreeNode((void*)(intptr_t)i, thumbnail))
                    {
                        AddButton(i);
                        ImGui::Text("Jump Id: %s", label);

                        if (strcmp(label, "_") == 0)
                        {
                            if (ImGui::Button("Copy last label"))
                            {
                                CopyLastLabelInfo(i);
                            }
                        }

                        IdOperator::ParseStringId<'_', MAX_ID_PART, NUM_OF_ID_PART>(label, currentId);
                        if (!IsNum(currentId[(int)ID_PART::COUNT])) {
                            strcpy(currentId[(int)ID_PART::COUNT], "0");
                        }
                        //if (ImGui::BeginCombo(chapterConstant, currentId[(int)ID_PART::CHAPTER], 0)) // The second parameter is the label previewed before opening the combo.
                        //{

                        //    for (int i = 0; i < chapterTable->GetSize(); i++)
                        //    {
                        //        bool is_selected = strcmp(currentId[(int)ID_PART::CHAPTER], chapterTable->GetRow(i)->Get(0)) == 0;
                        //        if (ImGui::Selectable(chapterTable->GetRow(i)->Get(0), is_selected)) {
                        //            strcpy(currentId[(int)ID_PART::CHAPTER], chapterTable->GetRow(i)->Get(0));

                        //        }
                        //        if (is_selected)
                        //            ImGui::SetItemDefaultFocus();   // Set the initial focus when opening the combo (scrolling + for keyboard navigation support in the upcoming navigation branch)

                        //    }

                        //    ImGui::EndCombo();
                        //}

                        //if (ImGui::BeginCombo(sceneConstant, currentId[(int)ID_PART::SCENE], 0)) // The second parameter is the label previewed before opening the combo.
                        //{

                        //    for (int i = 0; i < sceneTable->GetSize(); i++)
                        //    {
                        //        bool is_selected = strcmp(currentId[(int)ID_PART::SCENE], sceneTable->GetRow(i)->Get(0)) == 0;
                        //        if (ImGui::Selectable(sceneTable->GetRow(i)->Get(0), is_selected)) {
                        //            strcpy(currentId[(int)ID_PART::SCENE], sceneTable->GetRow(i)->Get(0));

                        //        }
                        //        if (is_selected)
                        //            ImGui::SetItemDefaultFocus();   // Set the initial focus when opening the combo (scrolling + for keyboard navigation support in the upcoming navigation branch)
                        //    }

                        //    ImGui::EndCombo();
                        //}

                        ImGui::InputText(idTitileConstant, currentId[(int)ID_PART::TITLE], MAX_ID_TITLE, ImGuiInputTextFlags_CharsNoBlank);

                        ImGui::InputText(idCountConstant, currentId[(int)ID_PART::COUNT], MAX_ID_COUNT, ImGuiInputTextFlags_CharsDecimal);

                        IdOperator::CombineStringId<'_', MAX_ID_PART, NUM_OF_ID_PART>(pLabel->m_labelId, currentId);
                        memset(currentId, '\0', NUM_OF_ID_PART*MAX_ID_COUNT);

                        //ImGui::InputTextWithHint(LabelConstant, "Enter label ID here", label, MAX_NAME);
                        ImGui::InputTextWithHint(LabelConstant, "Enter label ID here", label, MAX_ID);
                        ImGui::TextColored(ImVec4(1.0f, 0.0f, 1.0f, 1.0f), label);
                        ImGui::TreePop();
                    }
                    break;
                }

                case NodeType::End:
                {
                    StoryEnd* end = static_cast<StoryEnd*>(node);
                    assert(end != nullptr);

                    StoryLabel *pLabel = nullptr;
                    char decoration[] = " ======== ";
                    strcpy(thumbnail, decoration);
                    for (int j = i - 1; j >= 0; j--)
                    {
                        StoryNode* relatedNode = m_storyJson->GetNode(j);
                        if (relatedNode->m_nodeType == NodeType::Label)
                        {
                            pLabel = static_cast<StoryLabel*>(relatedNode);
                            label = pLabel->m_labelId;
                            strcat(thumbnail, "Label End: ");
                            strcat(thumbnail, order);
                            strcat(thumbnail, "  ");
                            strcat(thumbnail, label);
                            break;
                        }
                    }
                    if (pLabel == nullptr)
                    {
                        strcat(thumbnail, "Empty End");
                    }
                    strcat(thumbnail, decoration);

                    if (ImGui::TreeNode((void*)(intptr_t)i, thumbnail))
                    {
                        AddButton(i);
                        if (pLabel != nullptr)
                        {
                            ImGui::TextColored(ImVec4(0.0f, 1.0f, 0.0f, 1.0f), "End label: %s", pLabel->m_labelId);
                        }
                        else
                        {
                            ImGui::TextColored(ImVec4(1.0f, 0.0f, 0.0f, 1.0f), "Error! At least need a label before end");
                        }
                        ImGui::TreePop();
                    }
                }
                break;

                case NodeType::Exhibit:
                {
                    StoryExhibit* exhibit = static_cast<StoryExhibit*>(node);
                    assert(exhibit != nullptr);

                    char exhibitContent[16] = "Exhibit";
                    char exhibitPrefixConstant[16] = "Prefix";
                    char tmpExhibit[32] = "No Exhibit";
                    strcat(exhibitContent, order);
                    strcat(exhibitPrefixConstant, order);

                    exhibitID = exhibit->m_exhibitID;
                    exhibitPrefix = exhibit->m_exhibitPrefix;
                    strcpy(thumbnail, order);
                    strcat(thumbnail, "_Exhibit: ");
                    strcat(thumbnail, exhibitID);

                    if (ImGui::TreeNode((void*)(intptr_t)i, thumbnail))
                    {
                        ImVec4 color = colorRed;

                        AddButton(i);
                        ImGui::InputTextWithHint(exhibitContent, "Enter Exhibit ID here", exhibitID, MAX_EXHIBIT_NAME);
                        ImGui::InputTextWithHint(exhibitPrefixConstant, "Enter label prefix when select wrong exhibit here", exhibitPrefix, MAX_ID_TITLE);

                        if (strlen(exhibitID) == 0) {
                            strcpy(tmpExhibit, "Please Enter Exhibit ID");
                        }if(strlen(exhibitPrefix) == 0){
                            strcpy(tmpExhibit, "Please Enter Exhibit Prefix");
                        }else{
                            const StoryTable<EXHIBIT_COLUMN, EXHIBIT_TABLE_MAX_CONTENT>* const exhibitTable = StoryTableManager::Instance().GetExhibitTable();

                            for (int i = 0; i < exhibitTable->GetSize(); i++)
                            {
                                const StoryRow<EXHIBIT_COLUMN, EXHIBIT_TABLE_MAX_CONTENT>* const row = exhibitTable->GetRow(i);
                                if (strcmp(row->Get(0), exhibitID) == 0)
                                {
                                    if (strlen(row->Get(3)) == 0) {
                                        strcpy(tmpExhibit, "Lost Image File");
                                        break;
                                    }
                                    if (strlen(row->Get(2)) == 0) {
                                        strcpy(tmpExhibit, "Lost Exhibit Content");
                                        break;
                                    }
                                    strcpy(tmpExhibit, "Exhibit: ");
                                    strcpy(tmpExhibit, row->Get(1));
                                    color = colorGreen;
                                }
                            }
                        }

                        ImGui::TextColored(color, tmpExhibit);
                        ImGui::TreePop();
                    }
                }
                break;
                case NodeType::raiseEvent:
                {
                    StoryEvent* event = static_cast<StoryEvent*>(node);
                    assert(event != nullptr);

                    char eventContent[16] = "Event";
                    char tmpEvent[32] = "No Exhibit";
                    strcat(eventContent, order);

                    eventName = event->m_eventName;
                    EventType eventType = event->m_eventType;
                    strcpy(thumbnail, order);
                    strcat(thumbnail, "_Event: ");
                    strcat(thumbnail, eventTypeString[(int)eventType]);
                    strcat(thumbnail, "---");
                    strcat(thumbnail, eventName);
                    if (ImGui::TreeNode((void*)(intptr_t)i, thumbnail))
                    {
                        AddButton(i);

                        if (ImGui::BeginCombo("Event Type", eventTypeString[(int)eventType], 0))
                        {

                            for (int i = 0; i < EventTypeAmount; i++)
                            {
                                bool is_selected = eventType == (EventType)i;
                                if (ImGui::Selectable(eventTypeString[i], is_selected)) {
                                    event->m_eventType = (EventType)i;
                                }
                                if (is_selected)
                                    ImGui::SetItemDefaultFocus();   // Set the initial focus when opening the combo (scrolling + for keyboard navigation support in the upcoming navigation branch)
                            }

                            ImGui::EndCombo();
                        }
                        ImGui::InputTextWithHint(eventContent, "Enter Event ID here", eventName, MAX_EVENT_NAME);

                        ImGui::TextColored(ImVec4(0.0f, 1.0f, 0.0f, 1.0f), eventName);
                        ImGui::TreePop();
                    }
                }
                break;
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

        if (ImGui::Button("Add new exhibit")) {
            if (m_storyJson != nullptr) {
                m_storyJson->AddExhibit("", "");
                AddNotification("Add New Exhibit");
            }
        }
        ImGui::SameLine();

        if (ImGui::Button("Add new event")) {
            if (m_storyJson != nullptr) {
                m_storyJson->AddEvent("");
                AddNotification("Add New Event");
            }
        }
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
        ImGui::SameLine();
        if (ImGui::Button("Add new end")) {

            if (m_storyJson != nullptr)
            {
                m_storyJson->AddEnd();

                AddNotification("Add New End");
            }
        }

        {
            static char importContent[1024 * 32] = "\0";

            ImGui::Text("Import (Test function) MAX Content limit: 1024 * 16(line length limit)");
            static ImGuiInputTextFlags flags = ImGuiInputTextFlags_AllowTabInput;
            ImGui::InputTextMultiline("##source", importContent, IM_ARRAYSIZE(importContent), ImVec2(-FLT_MIN, ImGui::GetTextLineHeight() * 16), flags);
            if (ImGui::Button("import"))
            {
                bool result = ImportContentInternal(m_storyJson, importContent);
                if (!result)
                {
                    AddNotification("Import Failed. Please check the length of imported name and content");
                }
            }
        }
        //Update Auto Save
        StoryTimer<HeavenGateWindowStoryEditor>::Update();
    }


    void HeavenGateWindowStoryEditor::UpdateMenu()
    {

        if (ImGui::MenuItem("New")) {
            m_inputFileNamePopup->OpenWindow();
            m_inputFileNamePopup->SetCallbackAfterClickOk(&HeavenGateWindowStoryEditor::CallbackNewFile);


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
            SaveStoryFile();
        }
        if (ImGui::MenuItem("Export", "Ctrl+E")) {

            bool result = StoryFileManager::Instance().ExportStoryFile(m_storyJson);

            if (result)
            {
                AddNotification("Successful to Export File");
            }
            else
            {
                AddNotification("Failed to Export File. Please check console for more informations");
            }

        }
        if (ImGui::MenuItem("Rename"))
        {
            m_inputFileNamePopup->OpenWindow();
            m_inputFileNamePopup->SetCallbackAfterClickOk(&HeavenGateWindowStoryEditor::CallbackRenameFile);

        }




        ImGui::Separator();
        if (ImGui::MenuItem("Preview Window"))
              {
                  m_previewWindow->OpenWindow();

              }


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
            if (ImGui::Button("Insert New Exhibit")) {
                if (m_storyJson != nullptr) {
                    m_storyJson->InsertExhibit("", "", index);
                    AddNotification("Insert New Exhibit");
                }
            }
            ImGui::SameLine();
            if (ImGui::Button("Insert New Event")) {
                if (m_storyJson != nullptr) {
                    m_storyJson->InsertEvent("", index);
                    AddNotification("Insert New Event");
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
            if (ImGui::Button("Insert new end")) {
                if (m_storyJson != nullptr)
                {
                    m_storyJson->InsertEnd(index);
                    AddNotification("Insert New End");
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

    bool HeavenGateWindowStoryEditor::ImportContentInternal(StoryJson* const importedStory, const char* const needImportedContent)
    {
        char cache[1024] = "";
        int cacheLength = 0;
        bool isName = true;
        int index = -1;

        int i = 0;
        for (; i < 1024 * 32 && needImportedContent[i] != '\0'; i++)
        {
            if (needImportedContent[i] == '\n')
            {
                //new line
                int cacheLenth = strlen(cache);
                if (cacheLenth != 0)
                {
                    if (isName)
                    {
                        if (importedStory != nullptr)
                        {
                            if (!CheckStringLength(cache, MAX_NAME_LIMIT))
                            {
                                return false;
                            }
                            index = importedStory->AddWord(cache, "");
                            isName = false;
                        }
                    }
                    else
                    {
                        if (index > 0)
                        {
                            if (importedStory != nullptr)
                            {
                                StoryNode* node = importedStory->GetNode(index);
                                if (node != nullptr)
                                {
                                    if (!CheckStringLength(cache, MAX_CONTENT_LIMIT))
                                    {
                                        return false;
                                    }
                                    StoryWord* word = static_cast<StoryWord*>(node);
                                    strcpy(word->m_content, cache);
                                    isName = true;
                                }
                            }
                        }
                    }
                    memset(cache, '\0', 1024);
                    cacheLength = 0;
                }
            }
            else
            {
                cache[cacheLength] = needImportedContent[i];
                cacheLength++;
            }
        }
        return true;
    }

    int HeavenGateWindowStoryEditor::WordContentCallback(ImGuiInputTextCallbackData * data)
    {
        return 0;

        StoryWord compiledWord;
        strcpy(compiledWord.m_content, data->Buf);
       // m_previewWindow->SetPreviewWord(compiledWord);
//
//        vector<StoryJsonContentCompiler::Token*>tokens = StoryJsonContentCompiler::Instance().CompileToTokens(&compiledWord);
//
//        TableType currentState;
//        deque<TableType> editorState;
//        bool isReadCloseLabel;
//
//        isReadCloseLabel = false;
//        currentState = TableType::None;
//
//        //Tmp value
//        ImVec4 color(1.0f, 1.0f, 1.0f, 1.0f);
//
//        char tmpTip[MAX_COLUMNS_CONTENT_LENGTH];
//        memset(tmpTip, '\0', MAX_COLUMNS_CONTENT_LENGTH);
//
//        char tmpExhibit[EXHIBIT_TABLE_MAX_CONTENT];
//        memset(tmpExhibit, '\0', EXHIBIT_TABLE_MAX_CONTENT);
//
//        char tmpBgm[MAX_COLUMNS_CONTENT_LENGTH];
//        memset(tmpBgm, '\0', MAX_COLUMNS_CONTENT_LENGTH);
//
//        char tmpEffect[MAX_COLUMNS_CONTENT_LENGTH];
//        memset(tmpEffect, '\0', MAX_COLUMNS_CONTENT_LENGTH);
//
//        char tmpTachieCommand[NUM_OF_TACHIE_COMMAND][MAX_COLUMNS_CONTENT_LENGTH];
//        memset(tmpTachieCommand, '\0', NUM_OF_TACHIE_COMMAND * MAX_COLUMNS_CONTENT_LENGTH);
//
//        char tmpFontSize[MAX_COLUMNS_CONTENT_LENGTH];
//        memset(tmpFontSize, '\0', MAX_COLUMNS_CONTENT_LENGTH);
//
//        char tmpPause[MAX_COLUMNS_CONTENT_LENGTH];
//        memset(tmpPause, '\0', MAX_COLUMNS_CONTENT_LENGTH);
//
//        bool isExhibit = false;
//        //ImGui::EndChildFrame();
//        //ImGui::InputTextMultiline("Preivew", "", 100 - 4, ImVec2(-FLT_MIN, ImGui::GetTextLineHeight() * 1));
//
//        ImGui::Text("Preview:");
//
//        for (auto iter = tokens.cbegin(); iter != tokens.end(); iter++)
//        {
//            if ((*iter)->m_tokeType == StoryJsonContentCompiler::TokenType::TokenContent)
//            {
//                ImGui::TextColored(color, (*iter)->m_content);
//                ImGui::SameLine(0, 0);
//            }
//            else if ((*iter)->m_tokeType == StoryJsonContentCompiler::TokenType::TokenInstructor)
//            {
//                if (isReadCloseLabel)
//                {
//                    // Is closing state
//                    if (editorState.empty())
//                    {
//                        printf("Close Label is lack\n");
//                        continue;
//                    }
//
//                    currentState = editorState.back();
//                    editorState.pop_back();
//                    isReadCloseLabel = false;
//
//                    switch (currentState)
//                    {
//                    case HeavenGateEditor::TableType::None:
//                        break;
//                    case HeavenGateEditor::TableType::Font_Size:
//                        if (strlen(tmpFontSize) != 0) {
//                            ImGui::TextColored(colorGreen, "[End font size]");
//                        }
//                        else {
//                            ImGui::TextColored(colorRed, "!!Can not find font size");
//                        }
//                        ImGui::SameLine(0, 0);
//                        break;
//                    case HeavenGateEditor::TableType::Color:
//                        color = colorWhite;
//                        break;
//                    case HeavenGateEditor::TableType::Tips:
//                        ImGui::TextColored(color, "[Show Tip Here: %s]", tmpTip);
//                        ImGui::SameLine(0, 0);
//                        break;
//                    case HeavenGateEditor::TableType::Paint_Move:
//                        break;
//                    case HeavenGateEditor::TableType::Exhibit:
//                        if (isExhibit == true) {
//                            ImGui::TextColored(colorGreen, "[Get Exhibit Here: %s]", tmpExhibit);
//                            isExhibit = false;
//                        }
//                        break;
//                    case TableType::Pause:
//                        if (strlen(tmpPause) != 0) {
//                            ImGui::TextColored(colorGreen, "[End interval time]");
//                        }
//                        else {
//                            ImGui::TextColored(colorRed, "!!Can not find interval time");
//                        }
//                        ImGui::SameLine(0, 0);
//                        break;
//                    case TableType::Bgm:
//                        if (strlen(tmpBgm) != 0) {
//                            ImGui::TextColored(colorGreen, "[Play Bgm: %s]", tmpBgm);
//
//                        }
//                        else {
//                            ImGui::TextColored(colorRed, "!!Can not find bgm");
//                        }
//
//                        ImGui::SameLine(0, 0);
//                        break;
//                    case TableType::Effect:
//                        if (strlen(tmpEffect) != 0) {
//                            ImGui::TextColored(colorGreen, "[Play Effect: %s]", tmpEffect);
//
//                        }
//                        else {
//                            ImGui::TextColored(colorRed, "!!Can not find effect");
//                        }
//                        ImGui::SameLine(0, 0);
//                        break;
//                    case TableType::Tachie:
//                        if (strlen(tmpTachieCommand[0]) == 0) {
//                            ImGui::TextColored(colorRed, "!!Tchie is empty");
//                        }
//                        if (strlen(tmpTachieCommand[1]) == 0) {
//                            ImGui::TextColored(colorRed, "!!Tchie position is empty");
//                        }
//                        ImGui::TextColored(colorGreen, "[Display Tachie: %s] [Tachie Position: %s]", tmpTachieCommand[0], tmpTachieCommand[1]);
//                        ImGui::SameLine(0, 0);
//                        break;
//                    default:
//                        break;
//                    }
//                }
//                else
//                {
//                    // Is start state
//                    if (strcmp((*iter)->m_content, fontSizeTableString[(int)FontSizeTableLayout::Type]) == 0)
//                    {
//                        editorState.push_back(TableType::Font_Size);
//                    }
//                    if (strcmp((*iter)->m_content, colorTableString[(int)FontSizeTableLayout::Type]) == 0)
//                    {
//                        editorState.push_back(TableType::Color);
//                    }
//                    if (strcmp((*iter)->m_content, paintMoveTableString[(int)FontSizeTableLayout::Type]) == 0)
//                    {
//                        editorState.push_back(TableType::Paint_Move);
//                    }
//                    if (strcmp((*iter)->m_content, pauseTableString[(int)FontSizeTableLayout::Type]) == 0)
//                    {
//                        editorState.push_back(TableType::Pause);
//                    }
//                    if (strcmp((*iter)->m_content, tipTableString[(int)TipTableLayout::Type]) == 0)
//                    {
//                        editorState.push_back(TableType::Tips);
//                    }
//                    if (strcmp((*iter)->m_content, bgmTableString[(int)BgmTableLayout::Type]) == 0) {
//                        editorState.push_back(TableType::Bgm);
//                    }
//                    if (strcmp((*iter)->m_content, effectTableString[(int)EffectTableLayout::Type]) == 0) {
//                        editorState.push_back(TableType::Effect);
//                    }
//                    if (strcmp((*iter)->m_content, exhibitTableString[(int)ExhibitTableLayout::Type]) == 0) {
//                        editorState.push_back(TableType::Exhibit);
//                    }
//                    if (strcmp((*iter)->m_content, tachieTableString[(int)TachieTableLayout::Type]) == 0)
//                    {
//                        editorState.push_back(TableType::Tachie);
//                    }
//                }
//            }
//            else if ((*iter)->m_tokeType == StoryJsonContentCompiler::TokenType::TokenIdentity)
//            {
//
//                if (editorState.empty())
//                {
//                    printf("No Identity Exist. \n");
//                    continue;
//                }
//                switch (editorState.back())
//                {
//                case HeavenGateEditor::TableType::None:
//                    break;
//                case HeavenGateEditor::TableType::Font_Size:
//                {
//                    const StoryTable<FONT_SIZE_MAX_COLUMN>* const fontSizeTable = StoryTableManager::Instance().GetFontSizeTable();
//                    for (int i = 0; i < fontSizeTable->GetSize(); i++)
//                    {
//                        const StoryRow<FONT_SIZE_MAX_COLUMN>* const row = fontSizeTable->GetRow(i);
//                        if (strcmp(row->Get(0), (*iter)->m_content) == 0)
//                        {
//                            strcpy(tmpFontSize, row->Get(1));
//                        }
//                    }
//                    if (strlen(tmpFontSize) != 0) {
//                        ImGui::TextColored(colorGreen, "[Start font size: %s]", tmpFontSize);
//                    }
//                    else {
//                        ImGui::TextColored(colorRed, "Can not find font size");
//                    }
//                    ImGui::SameLine(0, 0);
//                }
//                break;
//                case HeavenGateEditor::TableType::Color:
//                {
//                    char colorAlias[MAX_COLUMNS_CONTENT_LENGTH];
//                    StoryTable<COLOR_MAX_COLUMN>* colorTable = StoryTableManager::Instance().GetColorTable();
//                    for (int j = 0; j < colorTable->GetSize(); j++)
//                    {
//                        strcpy(colorAlias, colorTable->GetRow(j)->Get(0));
//                        if (strcmp(colorAlias, (*iter)->m_content) == 0)
//                        {
//                            color = ImVec4(
//                                atoi(colorTable->GetRow(j)->Get(1)),
//                                atoi(colorTable->GetRow(j)->Get(2)),
//                                atoi(colorTable->GetRow(j)->Get(3)),
//                                atoi(colorTable->GetRow(j)->Get(4))
//                            );
//                            color = HeavenGateEditorUtility::ConvertRGBAToFloat4(color);
//                        }
//
//                    }
//
//                    break;
//                }
//                case HeavenGateEditor::TableType::Exhibit: {
//                    const StoryTable<EXHIBIT_COLUMN, EXHIBIT_TABLE_MAX_CONTENT>* const exhibitTable = StoryTableManager::Instance().GetExhibitTable();
//
//                    for (int i = 0; i < exhibitTable->GetSize(); i++)
//                    {
//                        const StoryRow<EXHIBIT_COLUMN, EXHIBIT_TABLE_MAX_CONTENT>* const row = exhibitTable->GetRow(i);
//                        if (strcmp(row->Get(0), (*iter)->m_content) == 0)
//                        {
//                            strcpy(tmpExhibit, row->Get(1));
//                        }
//                    }
//                    isExhibit = true;
//                    break;
//                }
//                case HeavenGateEditor::TableType::Tips:
//                {
//                    const StoryTable<TIP_MAX_COLUMN, TIP_TABLE_MAX_CONTENT>* const tipTable = StoryTableManager::Instance().GetTipTable();
//
//                    for (int i = 0; i < tipTable->GetSize(); i++)
//                    {
//                        const StoryRow<TIP_MAX_COLUMN, TIP_TABLE_MAX_CONTENT>* const row = tipTable->GetRow(i);
//                        if (strcmp(row->Get(0), (*iter)->m_content) == 0)
//                        {
//                            strcpy(tmpTip, row->Get(1));
//                        }
//                    }
//                }
//                break;
//                case HeavenGateEditor::TableType::Paint_Move:
//                    break;
//                case TableType::Pause:
//                {
//                    const StoryTable<PAUSE_MAX_COLUMN>* const pauseTable = StoryTableManager::Instance().GetPauseTable();
//                    for (int i = 0; i < pauseTable->GetSize(); i++)
//                    {
//                        const StoryRow<PAUSE_MAX_COLUMN>* const row = pauseTable->GetRow(i);
//                        if (strcmp(row->Get(0), (*iter)->m_content) == 0)
//                        {
//                            strcpy(tmpPause, row->Get(1));
//                        }
//                    }
//                    if (strlen(tmpPause) != 0) {
//                        ImGui::TextColored(colorGreen, "[Start interval time between words : %s]", tmpPause);
//                    }
//                    else {
//                        ImGui::TextColored(colorRed, "Can not find interval time");
//                    }
//                    ImGui::SameLine(0, 0);
//                }
//                break;
//                case TableType::Bgm:
//                {
//                    const StoryTable<BGM_MAX_COLUMN>* const bgmTable = StoryTableManager::Instance().GetBgmTable();
//                    for (int i = 0; i < bgmTable->GetSize(); i++)
//                    {
//                        const StoryRow<BGM_MAX_COLUMN>* const row = bgmTable->GetRow(i);
//                        if (strcmp(row->Get(0), (*iter)->m_content) == 0)
//                        {
//                            strcpy(tmpBgm, row->Get(1));
//                        }
//                    }
//                }
//                break;
//
//                case TableType::Effect:
//                {
//                    const StoryTable<EFFECT_MAX_COLUMN>* const effectTable = StoryTableManager::Instance().GetEffectTable();
//                    for (int i = 0; i < effectTable->GetSize(); i++)
//                    {
//                        const StoryRow<EFFECT_MAX_COLUMN>* const row = effectTable->GetRow(i);
//                        if (strcmp(row->Get(0), (*iter)->m_content) == 0)
//                        {
//                            strcpy(tmpEffect, row->Get(1));
//                        }
//                    }
//                }
//                break;
//                case TableType::Tachie:
//                {
//                    IdOperator::ParseStringId<'+', MAX_COLUMNS_CONTENT_LENGTH, NUM_OF_TACHIE_COMMAND>((*iter)->m_content, tmpTachieCommand);
//
//                    const StoryTable<TACHIE_MAX_COLUMN>* const tachieTable = StoryTableManager::Instance().GetTachieTable();
//                    const StoryTable<TACHIE_POSITION_MAX_COLUMN>* const tachiePositionTable = StoryTableManager::Instance().GetTachiePositionTable();
//                    for (int i = 0; i < tachieTable->GetSize(); i++)
//                    {
//                        const StoryRow<TACHIE_MAX_COLUMN>* const row = tachieTable->GetRow(i);
//                        if (strcmp(row->Get(0), tmpTachieCommand[0]) == 0)
//                        {
//                            strcpy(tmpTachieCommand[0], row->Get(1));
//                        }
//                    }
//                    for (int i = 0; i < tachiePositionTable->GetSize(); i++)
//                    {
//                        const StoryRow<TACHIE_POSITION_MAX_COLUMN>* const row = tachiePositionTable->GetRow(i);
//                        if (strcmp(row->Get(0), tmpTachieCommand[1]) == 0)
//                        {
//                            strcpy(tmpTachieCommand[1], row->Get(1));
//                            strcat(tmpTachieCommand[1], ",");
//                            strcat(tmpTachieCommand[1], row->Get(2));
//                        }
//                    }
//
//                }
//                break;
//                default:
//                    break;
//                }
//            }
//            else if ((*iter)->m_tokeType == StoryJsonContentCompiler::TokenType::TokenOpSlash)
//            {
//                //Start close
//                isReadCloseLabel = true;
//            }
//
//        }
//
//        ImGui::Text("");
//        ImGui::Separator();
        return 0;
    }




    void HeavenGateWindowStoryEditor::ShowStoryWord(StoryWord* word, int index)
    {
        assert(word != nullptr);
        assert(word->m_name != nullptr);
        assert(word->m_content != nullptr);

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
            //ImGui::InputTextWithHint(contentConstant, "Enter Content here", content, MAX_CONTENT, ImGuiInputTextFlags_CallbackAlways, WordContentCallback);
            ImGui::InputTextMultiline(contentConstant, content, MAX_CONTENT - 4, ImVec2(-FLT_MIN, ImGui::GetTextLineHeight() * 4), ImGuiInputTextFlags_None);

            if (ImGui::IsItemActive()){
                m_previewWindow->SetPreviewWord(*word);
            }
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
                if (m_messageBoxPopup)
                {
                    m_messageBoxPopup->SetMessage(strerror(errno));
                    m_messageBoxPopup->OpenWindow();
                }
            }

        }
    }

    void HeavenGateWindowStoryEditor::SaveStoryFile()
    {
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

    bool HeavenGateWindowStoryEditor::IsNum(const char * const content)
    {
        for (int i = 0; i < strlen(content); i++)
        {
            int tmp = (int)content[i];
            if (tmp >= 48 && tmp <= 57)
            {
                continue;
            }
            else
            {
                return false;
            }
        }
        return true;
    }

    void HeavenGateWindowStoryEditor::CallbackNewFile(const char* fileName)
    {

        char folderPath[MAX_FOLDER_PATH];

        //const char* fileName = m_inputFileNamePopup->GetFileName();
        if (fileName == nullptr || strlen(fileName) < 1)
        {
            printf("File name is not a valid value");
            return;
        }
        if (StoryFileManager::Instance().FromFileNameToFullPath(folderPath, fileName)) {
            StoryJson* story = StoryJsonManager::Instance().GetStoryJson();
            //If already have a file
            if (story->IsExistFullPath() == true) {
                StoryFileManager::Instance().SaveStoryFile(story);
                story->Clear();
            }
            else {
                //If story don`t loaded
                return;
            }
            story->SetFullPath(folderPath);
            //Default Node
            char labelID[MAX_ID];
            strcpy(labelID, fileName);
            strcat(labelID, "_0");
            story->AddLabel(labelID);
        }
        else {
            printf("Illegal File Name");
        }
    }

    void HeavenGateWindowStoryEditor::CallbackRenameFile(const char* fileName)
    {
        char folderPath[MAX_FOLDER_PATH];

        //const char* fileName = m_inputFileNamePopup->GetFileName();
        if (fileName == nullptr || strlen(fileName) < 1)
        {
            printf("File name is not a valid value");
            return;
        }
        if (StoryFileManager::Instance().FromFileNameToFullPath(folderPath, fileName)) {
            StoryJson* story = StoryJsonManager::Instance().GetStoryJson();

            story->SetFullPath(folderPath);

            SaveStoryFile();
        }
        else {
            printf("Illegal File Name");
        }

    }

    bool HeavenGateWindowStoryEditor::CheckStringLength(const char* string, int stringLengthLimit)
    {
        return strlen(string) <= stringLengthLimit;
    }

    void HeavenGateWindowStoryEditor::CopyLastLabelInfo(int index)
    {
        StoryNode* currentNode = m_storyJson->GetNode(index);
        if (currentNode->m_nodeType != NodeType::Label)
        {
            return;
        }
        StoryLabel* currentLabel = static_cast<StoryLabel*>(currentNode);
        for (int i = index - 1; i >= 0; i--)
        {
            StoryNode* node = m_storyJson->GetNode(i);
            if (node->m_nodeType == NodeType::Label)
            {
                StoryLabel* label = static_cast<StoryLabel*>(node);
                if (strlen(label->m_labelId) != 0 && strcmp(label->m_labelId, "_") != 0)
                {
                    char currentId[NUM_OF_ID_PART][MAX_ID_COUNT];
                    IdOperator::ParseStringId<'_', MAX_ID_PART, NUM_OF_ID_PART>(label->m_labelId, currentId);
                    if (!IsNum(currentId[(int)ID_PART::COUNT]) || strlen(currentId[(int)ID_PART::COUNT]) == 0) {
                        strcpy(currentId[(int)ID_PART::COUNT], "0");
                    }
                    int counter = 0;
                    counter = atoi(currentId[(int)ID_PART::COUNT]);
                    counter++;
                    char countBuffer[MAX_ID_COUNT];
#ifdef _WIN32
                    itoa(counter, countBuffer, 10);
#else
                    CharacterUtility::itoa(counter, countBuffer, 10);
#endif
                    strcpy(currentId[(int)ID_PART::COUNT], countBuffer);
                    IdOperator::CombineStringId<'_', MAX_ID_PART, NUM_OF_ID_PART>(currentLabel->m_labelId, currentId);

                    return;
                }
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

