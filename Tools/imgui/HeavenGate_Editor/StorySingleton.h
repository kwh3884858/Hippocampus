#ifndef StorySingleton_h
#define StorySingleton_h

#include <assert.h>
namespace HeavenGateEditor {


    template<typename T>
    class StorySingleton
    {
    public:
        //Note: Scott Meyers mentions in his Effective Modern
        //       C++ book, that deleted functions should generally
        //       be public as it results in better error messages
        //       due to the compilers behavior to check accessibility
        //       before deleted status
        StorySingleton(const StorySingleton&) = delete;
        void operator=(const StorySingleton&) = delete;

        //Derived class should have a public override destructor
        virtual ~StorySingleton() = default;

        //Initialize function, take the place of constructor
        virtual bool Initialize() = 0;

        //destroy function, take the  place of destructor
        virtual bool Shutdown() = 0;

        //Get singleton class, have and the only have one instance;
        static inline T& Instance() {

            static T* gpSingleton = nullptr;

            if (gpSingleton == nullptr)
            {
                gpSingleton = new T;
            }

            assert(gpSingleton);

            return *gpSingleton;
        }
    protected:
        // Constructor, should be used by derived
        // Deriver constructor should be public, otherwise instance can not get it
        StorySingleton() = default;
    };

}

#endif // StorySingleton_h
