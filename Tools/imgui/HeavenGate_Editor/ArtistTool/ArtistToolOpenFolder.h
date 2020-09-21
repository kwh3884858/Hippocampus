#ifdef _WIN32
#ifndef ArtistToolOpenFolder_h
#define ArtistToolOpenFolder_h

namespace  ArtistTool
{
    class ArtistToolOpenFolder 
    {
    public:
        ArtistToolOpenFolder() {}
        ~ArtistToolOpenFolder() {}

        static void OpenFolder(const wchar_t* const folderPath);
        static void OpenFolder(const char* const folderPath);
    protected:
    private:

    };

}

#endif /* ArtistToolOpenFolder_h */

#endif
