
    redirect(); //Appel de la fonction
    
    //Code de la Fonction
        function redirect() {
            var strinchemin = location.href.substring(0, location.href.lastIndexOf("/") + 1);
            var Viewname = location.href.substring(location.href.lastIndexOf("/")+1, location.href.length);
            strinchemin += "ReloadTimer?id="+Viewname;
            console.log(strinchemin);
            setTimeout("document.location = \'" + strinchemin+"'" , 600000); //60000s
            //setTimeout("document.location = 'production/Test/production'" , 6000); //36000s
        }
