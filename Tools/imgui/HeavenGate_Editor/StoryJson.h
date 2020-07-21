#ifndef StoryJson_h
#define StoryJson_h

#include <stdio.h>

#include <list>
#include "json.hpp"
#include "HeavenGateEditorConstant.h"


namespace HeavenGateEditor {
    using json = nlohmann::json;
    using std::list;


    class StoryNode;
    class StoryWord;
    class StoryLabel;
    class StoryJump;
    class StoryExhibit;
    class StoryEvent;

    enum class StructLayout :int;
    //enum class InfoLayout :int;

    extern char StructString[][MAX_ENUM_LENGTH];
    //extern char infoString[][MAX_ENUM_LENGTH];




    class StoryJson {

    public:
        StoryJson();
        StoryJson(const StoryJson& storyJson);
        StoryJson(StoryJson&& storyJson)noexcept;
        ~StoryJson();

        StoryJson& operator=(StoryJson&& storyJson)noexcept;

        int AddNode(StoryNode* const node);

        int AddWord(StoryWord* const word);
        int AddWord(const char* name, const char* content);
        int InsertWord(const char* name, const char* content, int index);

        int AddLabel(const char* labelName);
        int InsertLabel(const char* labelName, int index);

        int AddJump(const char* jumpName, const char* jumpContent);
        int InsertJump(const char* jumpName, const char* jumpContent, int index);

        int AddExhibit(const char* exhibitName);
        int InsertExhibit(const char* exhibitName, int index);

        int AddEvent(const char* eventName);
        int InsertEvent(const char* eventName, int index);

        int AddEnd();
        int InsertEnd(int index);

        void Swap(int lhs, int rhs);

        StoryNode* const GetNode(int index);
        const StoryNode* const GetNode(int index) const;

        void Remove(int index);
        StoryWord FindWord(int index)const;
        int Size() const;
        void Clear();
        bool Empty()const;

        //Nedd .json Suffix
        bool SetFileName(const char* const fileName);
        //Contain .json suffix
        bool GetFileName(char* const outFileName) const;

        void SetFullPath(const char* fullPath);
        const char* GetFullPath()const;
        bool IsExistFullPath()const;
        /*
                void SetChapter(const char* chapter) { strcpy(m_chapter, chapter); }
                const char *const GetChapter()const { return m_chapter; }
                char* GetChapter() { return const_cast<char*>(const_cast<const StoryJson*>(this)->GetChapter()); }

                void SetScene(const char* scene) { strcpy(m_scene, scene); }
                const char* const GetScene()const { return m_scene; }
                char* GetScene() { return const_cast<char*>(const_cast<const StoryJson*>(this)->GetScene()); }
        */
    private:
        list<StoryNode*> m_nodes;
        char m_fullPath[MAX_FOLDER_PATH];

        //char m_chapter[MAX_FOLDER_PATH];
        //char m_scene[MAX_FOLDER_PATH];

    };


    void to_json(json& j, const StoryJson& p);
    void from_json(const json& j, StoryJson& p);

    void ToJsonFactory(json& j, const StoryJson& p);
    void FromJsonFactory(const json& j, StoryJson & p);


    namespace IdOperator {
        //        void ParseStringId(const char* const pinStringId, char(*pOutIdArray)[MAX_ID_COUNT]);
        //        void CombineStringId(char* const pOutStringId, char(*pInIdArray)[MAX_ID_COUNT]);

        template<char delimiter, int PART_LENGTH, int NUM_OF_PART>
        void ParseStringId(const char* const pinStringId, char(*pOutIdArray)[PART_LENGTH]);

        template<char delimiter, int PART_LENGTH, int NUM_OF_PART>
        void CombineStringId(char* const pOutStringId, char(*pInIdArray)[PART_LENGTH]);
    }

    template<char delimiter, int PART_LENGTH, int NUM_OF_PART>
    void IdOperator::ParseStringId(const char* const pinStringId, char(*pOutIdArray)[PART_LENGTH])
    {
        int strLength = strlen(pinStringId);
        if (strLength < 1) {
            return;
        }

        char tmpPartId[PART_LENGTH];
        memset(tmpPartId, '\0', PART_LENGTH);

        int j = 0;
        int indexOfArray = 0;

        for (int i = 0; i < strLength; i++)
        {
            if (pinStringId[i] == delimiter)
            {
                strcpy(pOutIdArray[indexOfArray++], tmpPartId);
                j = 0;
                memset(tmpPartId, '\0', PART_LENGTH);
                if (indexOfArray == NUM_OF_PART) return;
            }
            else {
                tmpPartId[j++] = pinStringId[i];
            }
        }
        //Read last part element
        strcpy(pOutIdArray[indexOfArray++], tmpPartId);
    }

    template<char delimiter, int PART_LENGTH, int NUM_OF_PART>
    void IdOperator::CombineStringId(char* const pOutStringId, char(*pInIdArray)[PART_LENGTH])
    {
        char delimiters[2];
        memset(delimiters, '\0', 2);
        delimiters[0] = delimiter;

        memset(pOutStringId, '\0', PART_LENGTH * NUM_OF_PART + NUM_OF_PART - 1);
        for (int i = 0; i < NUM_OF_PART - 1; i++)
        {
            strcat(pOutStringId, pInIdArray[i]);
            strcat(pOutStringId, delimiters);
        }
        strcat(pOutStringId, pInIdArray[NUM_OF_PART - 1]);
    }

}

#endif /* StoryJson_h */
