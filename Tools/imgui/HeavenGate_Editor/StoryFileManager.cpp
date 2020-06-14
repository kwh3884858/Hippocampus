#include "StoryFileManager.h"
#include "HeavenGateEditorUtility.h"
#include "StoryJson.h"
#include "StoryTable.h"
#include "CharacterUtility.h"

#include "StoryJsonChecker.h"
#include "StoryJsonContentCompiler.h"

#include "StoryTimer.h"

//#ifdef _WIN32

#include <dirent.h>


//#else
//#include <dirent.h>
//
//
//#endif // _WIN32

namespace HeavenGateEditor {


    bool StoryFileManager::LoadStoryFile(const char* pPath, StoryJson* pStoryJson)
    {
        std::ifstream fins;
        
        char charBuffer = '\0';
        vector<char> contentBuffer;

        char tcontent[MAX_FULL_CONTENT];
        memset(tcontent, 0, sizeof(tcontent));

        int i = 0;

        //memset(content, 0, sizeof(content));

        if (pPath == nullptr || pStoryJson == nullptr)
        {
            return false;
        }
        
        fins.open(pPath);
        if (fins.peek() == EOF) {

            std::cerr << "Error: File is empty.";
            return false;

        }
        // If it could not open the file then exit.
        if (!fins.fail())
        {
            
            while (!fins.eof())
            {
                //fins >> tcontent[i++];
                fins >>  charBuffer;
                contentBuffer.push_back(charBuffer);
            }

            fins.close();
        }
        else
        {
            std::cerr << "Error: " << strerror(errno);
            return false;
        }

        char* content = new char[contentBuffer.size()];
        std::copy(contentBuffer.cbegin(), contentBuffer.cend(), content);

        // The last character will push twish, because EOF will not make different with
        //original character, so we should set the last one to terminal character to
        //finish the sentense
        content[contentBuffer.size() - 1] = '\0';

        try {
            json a = json::parse(content);
            *pStoryJson = std::move(a);
            std::cout << a;
        }
        catch (json::parse_error &e)
        {
            std::cerr << e.what() << std::endl;
        }
       


       /* *pStoryJson = std::move(a);*/
        pStoryJson->SetFullPath(pPath);

        
        fins.close();

        return true;
    }

    bool StoryFileManager::SaveStoryFile(StoryJson* const pStoryJson) const
    {
        if (pStoryJson == nullptr) {
            return false;
        }
        char filePath[MAX_FOLDER_PATH];

        //Rename Start
        RenameStoryFileByChapterAndScene(pStoryJson);
        //Rename End

        strcpy(filePath, pStoryJson->GetFullPath());

        if (strlen(filePath) <= 0) {
            return false;
        }

        bool result = StoryJsonChecker::Instance().CheckJsonStory(pStoryJson);
        if (!result)
        {
            return false;
        }


        json tmpJson = *pStoryJson;

        std::ofstream outputFileStream(filePath);
        outputFileStream << tmpJson << std::endl;

        outputFileStream.close();

        //Initialize();

        return true;
    }
    bool StoryFileManager::AutoSaveStoryFile(StoryJson* const pStoryJson) const
    {
        char filePath[MAX_FOLDER_PATH];
        char originalFilePath[MAX_FOLDER_PATH];
        bool isSuccessful = false;

        if (pStoryJson == nullptr) {
            return false;
        }
        strcpy(originalFilePath, pStoryJson->GetFullPath());

        //Rename and put it to auto save folder
        RenameStoryFileByTimeAndPutAutoSaveFolder(pStoryJson);

        strcpy(filePath, pStoryJson->GetFullPath());

        if (strlen(filePath) <= 0) {
            return false;
        }

        //No gramme check
        json tmpJson = *pStoryJson;
        std::ofstream outputFileStream(filePath);

        if (!outputFileStream.fail())
        {
            outputFileStream << tmpJson << std::endl;
            isSuccessful = true;
        }
        else
        {
            std::cerr << "Error: " << strerror(errno);
        }

        outputFileStream.close();

        if (strlen(originalFilePath) != 0)
        {
            pStoryJson->SetFullPath(originalFilePath);
        }
        return isSuccessful;
    }

    bool StoryFileManager::ExportStoryFile(StoryJson* const pStoryJson) const
    {
        const char STORY_FOLDER[] = "Storys";
        char tmpPath[MAX_FOLDER_PATH];
        const int folderLength = strlen(STORY_FOLDER);

        if (pStoryJson == nullptr) {
            return false;
        }

        //Rename Start
        RenameStoryFileByChapterAndScene(pStoryJson);
        //Rename End

        char filePath[MAX_FOLDER_PATH];
        strcpy(filePath, pStoryJson->GetFullPath());

        if (strlen(filePath) <= 0) {
            return false;
        }

        bool result = StoryJsonChecker::Instance().CheckJsonStory(pStoryJson);
        if (!result)
        {
            return false;
        }

        int pos = CharacterUtility::Find(filePath, strlen(filePath), STORY_FOLDER, strlen(STORY_FOLDER));
        if (pos == -1)
        {
            return true;
        }
        int posNeedMove = pos + folderLength;
        strcpy(tmpPath, filePath + posNeedMove);
        strcpy(filePath + pos, STORY_EXPORT_FOLDER);
        strcat(filePath, tmpPath);

        StoryJson copyStory(*pStoryJson);
        StoryJsonContentCompiler::Instance().Compile(&copyStory);
        json tmpJson = copyStory;

        std::ofstream o(filePath);
        o << tmpJson << std::endl;

        o.close();

        //Initialize();

        return true;
    }

    void StoryFileManager::InitFileList(char(*pOutFileList)[MAX_FOLDER_PATH], int* maxFileCount)
    {

        char exePath[MAX_FOLDER_PATH];
        DIR *dir;
        struct dirent *ent;
        int fileCount = 0;


        HeavenGateEditorUtility::GetStoryPath(exePath);
        printf("Current Path:%s", exePath);

        if ((dir = opendir(exePath)) != NULL) {

            /* print all the files and directories within directory */
            while ((ent = readdir(dir)) != NULL) {

                printf("%s\n", ent->d_name);
                //Unity meta files
                if (CharacterUtility::Find(
                    ent->d_name,
                    strlen(ent->d_name),
                    ".meta",
                    strlen(".meta")) != -1)
                {
                    continue;
                }

                //MacOS system files
                if (CharacterUtility::Find(
                    ent->d_name,
                    strlen(ent->d_name),
                    ".DS_Store",
                    strlen(".DS_Store")) != -1)
                {
                    continue;
                }

                strcpy(pOutFileList[fileCount], ent->d_name);
                fileCount++;
            }
            closedir(dir);

            *maxFileCount = fileCount;
        }
        else {
            /* could not open directory */
            perror("");
            printf("Can`t open story folder");
            *maxFileCount = -1;
        }

    }

    //bool StoryFileManager::GetIsSaveFileExist()
    //{
    //    return m_isSaveFileExist;
    //}

    //char * StoryFileManager::GetNewFileName()
    //{
    //    return const_cast<char*>(const_cast<const StoryFileManager*>(this)->GetNewFileName());
    //}

    //const char * StoryFileManager::GetNewFileName() const
    //{
    //    return m_newFileName;
    //}

    //char * StoryFileManager::GetNewFilePath()
    //{
    //    return const_cast<char *>(const_cast<const StoryFileManager*>(this)->GetNewFilePath());
    //}

    //const char * StoryFileManager::GetNewFilePath() const
    //{
    //    return m_newFilePath;
    //}

    /*   void StoryFileManager::Initialize()
     {
     memset(m_newFilePath, 0, sizeof(m_newFilePath));
     memset(m_newFileName, 0, sizeof(m_newFileName));
     }*/

     //bool StoryFileManager::IsNewFilePathExist() const
     //{
     //    if (strlen(m_newFilePath) > 0) {
     //        return true;
     //    }
     //    else {
     //        return false;
     //    }
     //}

    bool StoryFileManager::FromFileNameToFullPath(char * filePath, const char * fileName) const
    {
        int length = static_cast<int>(strlen(fileName));

        if (length <= 0 || length >= MAX_FILE_NAME) {
            return false;
        }

        memset(filePath, 0, MAX_FOLDER_PATH);

        char storyPath[MAX_FOLDER_PATH];

        HeavenGateEditorUtility::GetStoryPath(storyPath);
        strcat(filePath, storyPath);

#ifdef _WIN32
        strcat(filePath, "\\");
#else
        strcat(filePath, "/");
#endif
        strcat(filePath, fileName);

        strcat(filePath, ".json");

        return true;
    }



    void StoryFileManager::GetFileContent(char* pFullPath, char* pOutContent)
    {

        /*  assert(MAX_CONTENT == sizeof(pOutContent));*/

        memset(pOutContent, '\0', MAX_FULL_CONTENT);

        std::ifstream fin;

        fin.open(pFullPath);

        // If it could not open the file then exit.
        if (!fin.fail())
        {
            int i = 0;
            while (!fin.eof() && i < MAX_FULL_CONTENT)
            {
                if (i >= MAX_FULL_CONTENT) {
                    std::cerr << "Out of max content limit";
                }
                fin >> pOutContent[i++];
            }

            fin.close();
        }
    }


    bool StoryFileManager::RenameStoryFileByChapterAndScene(StoryJson* const pStoryJson) const {

        char filePath[MAX_FOLDER_PATH];
        strcpy(filePath, pStoryJson->GetFullPath());

        const char* const pChapater = pStoryJson->GetChapter();
        const char* const pScene = pStoryJson->GetScene();

        if (pChapater == nullptr || pScene == nullptr) {
            return false;
        }

        bool isChapterExist = CharacterUtility::Find(filePath, strlen(filePath), pChapater, strlen(pChapater)) != -1;
        bool isSceneExist = CharacterUtility::Find(filePath, strlen(filePath), pScene, strlen(pScene)) != -1;

        if (!isChapterExist || !isSceneExist) {
            //Reform file name, add ID of chapter and scene.
            char newFileName[MAX_FILE_NAME] = { '\0' };
            char oldFileName[MAX_FILE_NAME] = { '\0' };

            pStoryJson->GetFileName(oldFileName);

            int pos = CharacterUtility::FindLast(oldFileName, strlen(oldFileName), "_", 1);

            if (pos != -1) {
                strcpy(oldFileName, oldFileName + pos);
            }

            strcpy(newFileName, pChapater);
            strcat(newFileName, "_");
            strcat(newFileName, pScene);
            strcat(newFileName, "_");
            strcat(newFileName, oldFileName);

            pStoryJson->SetFileName(newFileName);
        }

        return true;

    }

    bool StoryFileManager::RenameStoryFileByTimeAndPutAutoSaveFolder(StoryJson* const pStoryJson) const
    {
        char timeName[MAX_FILE_NAME] = { '\0' };

        char newFileName[MAX_FILE_NAME] = { '\0' };

        sprintf(timeName, "%d", StoryTimer<StoryFileManager>::GetTime());

        strcat(newFileName, AUTO_SAVE_FOLDER);
        strcat(newFileName, DELIMITER);
        strcat(newFileName, timeName);
        strcat(newFileName, ".json");

        pStoryJson->SetFileName(newFileName);

        return true;

    }

    //void StoryFileManager::SetNewFilePath(const char * filePath)
    //{
    //    strcpy(m_newFilePath, filePath);
    //}

}
