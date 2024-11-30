using BlazingQuiz.Shared;

namespace BlazingQuiz.Mobile;

public class MobilePlatform :IPlatform
{
    public bool IsMobileApp => true;
    public bool IsWeb => false;
}