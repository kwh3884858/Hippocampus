#pragma once
#ifndef StoryFileManager_h
#define StoryFileManager_h

#include "HeavenGateEditorConstant.h"
#include "StoryTableDefine.h"

#include <fstream>
#include <iostream>
#include "nlohmann/json.hpp"

#include "StorySingleton.h"

namespace HeavenGateEditor {

    using json = nlohmann::json;

    class StoryJson;

    class StoryFileManager final : public StorySingleton<StoryFileManager>
    {

    public:
        StoryFileManager() = default;
        virtual ~StoryFileManager() override = default;
        //initialize function, take the place of constructor
        virtual bool Initialize() override { return true; };
        //destroy function, take the  place of destructor
        virtual bool Shutdown() override { return true; };

        //TODO
        //bool CreateStoryFIle();
        bool LoadStoryFile(const char* pPath, StoryJson* pStoryJson);
        bool SaveStoryFile( StoryJson* const pStoryJson) const;
        bool AutoSaveStoryFile(StoryJson* const pStoryJson)const;
        bool ExportStoryFile( StoryJson* const pStoryJson) const;

        template<int column, int MAX_CONTENT_LENGTH = MAX_COLUMNS_CONTENT_LENGTH>
        bool LoadTableFile(const char* pPath, StoryTable<column, MAX_CONTENT_LENGTH>* const pTableJson)const;
        template<int column, int MAX_CONTENT_LENGTH = MAX_COLUMNS_CONTENT_LENGTH>
        bool SaveTableFile(const char* pPath, const StoryTable<column, MAX_CONTENT_LENGTH>* const pTableJson)const;

        void InitFileList(char(*pOutFileList)[MAX_FOLDER_PATH], int* maxFileCount);

        //Without file suffix
        bool FromFileNameToFullPath(char * filePath, const char* fileName) const;
        void GetFileContent(char* pFullPath, char* pOutContent);
    private:
        void InitAutoSaveFolder() const;
        //bool RenameStoryFileByChapterAndScene(StoryJson* const pStoryJson) const;
        bool RenameStoryFileByTimeAndPutAutoSaveFolder(StoryJson* const pStoryJson)const;
        void SortFiles(char (*fileList)[MAX_FOLDER_PATH], int fileCount);
        void SortFilesInternal(char (*fileList)[MAX_FOLDER_PATH], int pivot, int left, int right);
    };


    template<int column, int MAX_CONTENT_LENGTH>
    bool StoryFileManager::LoadTableFile(const char* pPath,
        StoryTable<column, MAX_CONTENT_LENGTH>*const pTableJson) const
    {
        std::ifstream fins;
        char content[MAX_FULL_CONTENT];
        memset(content, 0, sizeof(content));

        if (pPath == nullptr || pTableJson == nullptr)
        {
            return false;
        }

        fins.open(pPath);

        // If it could not open the file then exit.
        if (!fins.fail())
        {
            int i = 0;
            fins >> std::noskipws;
            while (!fins.eof())
            {
                fins >> content[i++];
            }

            fins.close();
        }
        else
        {
            std::cerr << "Error: " << strerror(errno);
            return false;
        }

        json a = json::parse(content);


        *pTableJson = std::move(a);

        std::cout << a;
        fins.close();

        return true;
    }

    template<int column, int MAX_CONTENT_LENGTH>
    bool StoryFileManager::SaveTableFile(const char* pPath,
        const StoryTable<column, MAX_CONTENT_LENGTH>* const pTableJson) const
    {
        if (pPath == nullptr || pTableJson == nullptr)
        {
            return false;
        }

        //char filePath[MAX_FOLDER_PATH];
        //strcpy(filePath, pTableJson->GetFullPath());

        if (strlen(pPath) <= 0) {
            return false;
        }

        json j_test = *pTableJson;
        std::ofstream o(pPath);
        o << j_test << std::endl;

        o.close();

        //Initialize();

        return true;
    }
}

#endif /* StoryFileManager_h */
