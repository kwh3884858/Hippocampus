#pragma once
#ifndef HeavenGateEditorNodeGraphExample_h
#define HeavenGateEditorNodeGraphExample_h

#include "HeavenGateEditorBaseWindow.h"

#include "HeavenGateEditorConstant.h"
namespace Example {
    class ExampleBase;
}
namespace HeavenGateEditor {


    class HeavenGateEditorNodeGraphExample : public HeavenGateEditorBaseWindow
    {
        WINDOW_DECLARE("HeavenGateEditorNodeGraphExample", Window_Type::MainWindow)
    public:
        HeavenGateEditorNodeGraphExample();
        virtual ~HeavenGateEditorNodeGraphExample() override;
        virtual void Initialize() override;
        virtual void Shutdown() override;

    protected:
        
        virtual void UpdateMainWindow() override;
        virtual void UpdateMenu()override;


    private:
        void OpenExample(int index);

        Example::ExampleBase* m_currentExample;
        Example::ExampleBase* m_exampleArray[4];
    };

}
#endif /* HeavenGateEditorNodeGraphExample_h */
