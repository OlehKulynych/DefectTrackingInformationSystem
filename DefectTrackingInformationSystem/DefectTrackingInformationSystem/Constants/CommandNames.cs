namespace DefectTrackingInformationSystem.Constants
{
    public class CommandNames
    {
       
        public const string StartCommand = "/start";

        //Input Defect Commands
        public const string InputDefectCommand = "/add-defect";
        public const string InputNumberRoomsCommand = "input-number-rooms";
        public const string InputDecscriptionCommand = "input-description";
        public const string InputImageDefectCommand = "input-image-defect";
        public const string StartInputImageDefectCommand = "start-input-image-defect";
        public const string FinishInputDefectCommand = "finish-input-defect";

        //Get Commands
        public const string GetDefectsCommand = "/get-defects";

        //Fix Defect Command
        public const string FixDefectCommand = "/fix-defect";
        public const string FinishFixDefectCommand = "finish-fix";




    }
}
