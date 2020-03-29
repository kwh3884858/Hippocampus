#pragma once
namespace  Example {
    class ExampleBase
    {
    public:
        ExampleBase();
        ~ExampleBase();

        virtual void NodeEditorInitialize() = 0;
        virtual void NodeEditorShow() = 0;
        virtual void NodeEditorShutdown() = 0;
    };

}
