${
    Template(Settings settings){
        settings.OutputFilenameFactory = (file) => {
            var name = file.Name;
            name = name.Substring(0, name.IndexOf("."));
            name = String.Join("", name.Select((x,i)=>$"{(i > 0 && Char.IsUpper(x) ? "-" : "")}{x}")).ToLower();
            return name.ToLower();
        };
        settings.OutputExtension = ".tst.gen";
    }

    string ParentClass(Class c){
        return c.BaseClass != null ? "extends I" + c.BaseClass.Name : "";
    }
}
$Classes(*Model)[export interface I$Name$TypeParameters $ParentClass{
    $Properties(p=>!p.Type.IsEnum && !p.Type.IsPrimitive && (!p.Type.IsGeneric || p.Type.TypeArguments.All(z=>!z.IsEnum)))[$name: I$Type;
    ]$Properties(p=>p.Type.IsEnum || p.Type.IsPrimitive || !(!p.Type.IsGeneric || p.Type.TypeArguments.All(z=>!z.IsEnum)))[$name: $Type;
    ]
}

]$Enums(*)[export enum $Name { $Values[
    $Name = $Value][,]
}

]