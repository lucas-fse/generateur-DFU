using System;
        using System.Collections.Generic;
        using System.Net;
        using System.Web.Mvc;
        using System.DirectoryServices.Protocols;

namespace GenerateurDFUSafir.Controllers
{
    public class LDAPController : Controller
    {
        public ActionResult Index()
        {
            var utilisateurs = new List<UtilisateurLDAP>();

            try
            {
                string ldapServer = "192.168.1.232";
                int port = 389;
                string baseDn = "OU=Utilisateurs du domaine,DC=JAYELECTRONIQUE,DC=ORG";

                string username = "CN= Ldap User,OU=Comptes_Admin,DC=JAYELECTRONIQUE,DC=ORG";
                string password = "38721!Grenoble";

                // Création des crédentials
                var credential = new NetworkCredential(username, password);

                // Création de l'objet d'identification du LDAP
                var ldapIdentifier = new LdapDirectoryIdentifier(ldapServer, port);

                // Connection au LDAP avec le compte et on préshot si Kerberos est implémenté
                var connection = new LdapConnection(ldapIdentifier, credential, AuthType.Basic);
                connection.Bind();

                var request = new SearchRequest(
                    baseDn,
                    "(objectClass=person)",
                    SearchScope.Subtree,
                    new[] { "givenName", "sn", "mail" }
                );

                var response = (SearchResponse)connection.SendRequest(request);

                foreach (SearchResultEntry entry in response.Entries)
                {
                    utilisateurs.Add(new UtilisateurLDAP
                    {
                        Prenom = entry.Attributes["givenName"]?[0]?.ToString(),
                        Nom = entry.Attributes["sn"]?[0]?.ToString(),
                        Email = entry.Attributes["mail"]?[0]?.ToString()
                    });
                }
            }
            catch (Exception ex)
            {
                ViewBag.Erreur = "Erreur lors de la connexion LDAP : " + ex.Message;
            }

            return View(utilisateurs);
        }
    }

    public class UtilisateurLDAP
    {
        public string Prenom { get; set; }
        public string Nom { get; set; }
        public string Email { get; set; }
    }
}





