<script language="javascript">
    function calculSommeByDay()
    {
            var nbLignes = document.getElementById("tab").rows.length;
            document.getElementById('SommeLundi').value = 0;
            document.getElementById('SommeMardi').value = 0;
            document.getElementById('SommeMercredi').value = 0;
            document.getElementById('SommeJeudi').value = 0;
            document.getElementById('SommeVendredi').value = 0;
            document.getElementById('SommeSamedi').value = 0;
            document.getElementById('SommeDimanche').value = 0;

            for (i = 2 ; i < nbLignes-1 ; i++)
            {
                document.getElementById('SommeLundi').value = parseFloat(document.getElementById('SommeLundi').value) + parseFloat(document.getElementById('Lundi' + i).value);
                document.getElementById('SommeMardi').value = parseFloat(document.getElementById('SommeMardi').value) + parseFloat(document.getElementById('Mardi' + i).value);
                document.getElementById('SommeMercredi').value = parseFloat(document.getElementById('SommeMercredi').value) + parseFloat(document.getElementById('Mercredi' + i).value);
                document.getElementById('SommeJeudi').value = parseFloat(document.getElementById('SommeJeudi').value) + parseFloat(document.getElementById('Jeudi' + i).value);
                document.getElementById('SommeVendredi').value = parseFloat(document.getElementById('SommeVendredi').value) + parseFloat(document.getElementById('Vendredi' + i).value);
                document.getElementById('SommeSamedi').value = parseFloat(document.getElementById('SommeSamedi').value) + parseFloat(document.getElementById('Samedi' + i).value);
                document.getElementById('SommeDimanche').value = parseFloat(document.getElementById('SommeDimanche').value) + parseFloat(document.getElementById('Dimanche' + i).value);
            }
    }
 </script>