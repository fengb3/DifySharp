namespace DifySharp.Completion.Application;

public record Parameters
{
    public record ResponseBody(
        string                        OpeningStatement,
        SuggestedQuestionsAfterAnswer SuggestedQuestionsAfterAnswer,
        SpeechToText                  SpeechToText,
        RetrieverResource             RetrieverResource,
        AnnotationReply               AnnotationReply,
        List<UserInputForm>           UserInputForm,
        FileUpload                    FileUpload,
        SystemParameters              SystemParameters
    );

    public record SuggestedQuestionsAfterAnswer(bool Enabled);

    public record SpeechToText(bool Enabled);

    public record RetrieverResource(bool Enabled);

    public record AnnotationReply(bool Enabled);

    public record UserInputForm(TextInput TextInput, Paragraph Paragraph, Select Select);

    public record TextInput(
        string Label,
        string Variable,
        bool   Required,
        string Default
    );

    public record Paragraph(
        string Label,
        string Variable,
        bool   Required,
        string Default
    );

    public record Select(
        string       Label,
        string       Variable,
        bool         Required,
        List<string> Options
    );

    public record FileUpload(Image Image);

    public record Image(
        bool         Enabled,
        int          NumberLimits,
        string       Detail,
        List<string> TransferMethods
    );

    public record SystemParameters(
        int FileSizeLimit,
        int ImageFileSizeLimit,
        int AudioFileSizeLimit,
        int VideoFileSizeLimit
    );
}