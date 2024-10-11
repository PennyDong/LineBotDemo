namespace API.Dtos
{
    public class ActionDto
    {
        public string Type { get; set; }
        public string? Label { get; set; }

        //Postback action.
        public string? Data { get; set; } 
        public string? DisplayText { get; set; } 
        public string? InputOption { get; set; } 
        public string? FillInText { get; set; }

        //Message action

        public string? Text { get; set; }

        //Uri action
        public string? Uri { get; set; }
        public UriActionAltUriDto? AltUri { get; set; }

        

        //datetime picker action

        public string? Mode { get; set; }
        public string? Initial { get; set; }
        public string? Max { get; set; }
        public string? Min { get; set; }


        // rich menu switch action
        public string? RichMenuAliasId { get; set; }
    }
    public class UriActionAltUriDto
        {
            public string Desktop { get; set; }
        }
}

//richmenu-adc061da6ed4ec33f6b7c72b0b786ca7
//richmenu-b710e569a916d9f3d2651255b23a69a8

//"richmenu-0ead13cc952af16fe8059d6a10b62ac7"
//"richMenuId":"richmenu-e024e4b1417adad575ab4d93cd77015c"