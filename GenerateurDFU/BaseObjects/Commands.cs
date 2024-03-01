using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JAY.PegaseCore
{
    /// <summary>
    /// Toutes les constantes des messages de commandes
    /// </summary>
    public class Commands
    {
        // constantes
        public const String CMD_RUN_FIRST_FUNCTION = "R1CMD";               // Commande spécifiant que l'interface initiale vient de s'executée
        public const String CMD_MAJ_LANGUAGE = "MAJLa";                     // Commande spécifiant que la langue doit être mise à jour

        public const String CMD_CONNECT_CLICK = "COClick";                  // Le bouton de déconnexion vient d'être cliqué
        public const String CMD_MENU_MO_CLICK = "MOClick";                  // Le bouton du menu MO vient d'être cliqué
        public const String CMD_MENU_DECONNECT_CLICK = "DECOClick";         // Le bouton de déconnexion vient d'être cliqué
        public const String CMD_MENU_DECONNECT_CLICKALL = "DECOClickAll";         // sortie de l'outil
        public const String CMD_MENU_DECONNECT_WITHOUTSAVE = "DecoWSave";   // Déconnecter sans sauvegarder
        public const String CMD_MENU_XMLEDITOR = "XMLEClick";               // Le bouton de l'éditeur XML vient d'être cliqué
        public const String CMD_GROUP_MENU_CLICK = "GMClick";               // Un bouton GroupMenu a été cliqué
        public const String CMD_MENU_PRODUCTION_CLICK = "ProdClick";        // Le bouton du menu production a été cliqué
        public const String CMD_MENU_MAJ_MOMT_CLICK = "MAJ_MOMT";           // La mise à jour MO / MT
        public const String CMD_MENU_MAJ_MO_CLICK = "MAJ_MO";           // La mise à jour MO / MT
        public const String CMD_MENU_EDIT_VARIABLES_CLICK = "EdVarClick";   // Le bouton de menu Edit Variables a été cliqué
        public const String CMD_MENU_CHECKMATERIAL_CLICK = "CheckMClick";   // Le bouton de menu Check Materiel a été cliqué
        public const String CMD_MENU_REPORT_CLICK = "ReportClick";          // Le bouton de menu Report a été cliqué
        public const String CMD_MENU_WHC_CLICK = "WHCClick";                // Le bouton de menu WHC a été cliqué
        public const String CMD_EASY_CONFIG = "EConfigClick";               // Le bouton de menu Configuration facile a été cliqué
        public const String CMD_MENU_DESCRIPTIF = "MenuDescriptif";         // Le bouton du menu pour le descriptif

        public const String CMD_VALUE_CHANGED = "ValMAJ";                   // Une valeur a été mise à jour
        public const String CMD_ACTIVATE_MENU = "ActMenu";                  // Activer le menu
        public const String CMD_DESACTIVATE_MENU = "DesActMenu";            // Désactiver le menu

        public const String CMD_MAJ_INFO1 = "MAJINFO1";                     // Mettre à jour l'affichage du retour d'info1
        public const String CMD_MAJ_INFO2 = "MAJINFO2";                     // Mettre à jour l'affichage du retour d'info2
        public const String CMD_MAJ_INFO3 = "MAJINFO3";                     // Mettre à jour l'affichage du retour d'info3

        public const String CMD_CREATEFILE = "CFile";                       // Commande permettant la création d'un nouveau fichier iDialog

        public const String CMD_VIEWISLOADED = "LVIEW";                     // Commande spécifiant qu'une vue vien d'être chargée
        public const String CMD_MAJFIRMWARE = "MAJFW";                      // Commande spécifiant qu'une mise à jour du firmwar d'un produit est demandé
        public const String CMD_MAJFIRMWAREFROMDFU = "MAJFWFROMDFU";
        public const String CMD_ALLOW_RECUPERATION = "AllowRecup";          // command spécifiant que le mode récupération d'un produit est possible
        public const String CMD_APP_CLOSE = "AppClose";                     // commande de fermeture de l'application

        public const String CMD_WAIT_ON = "WON";                            // L'application est en attente
        public const String CMD_WAIT_OFF = "WOFF";                          // L'application n'attend plus
        public const String CMD_LOAD = "LOAD";                              // La commande 'Lire' est appelée
        public const String CMD_LOAD_FILE = "LOAD_FILE";                              // La commande 'Lire' est appelée

        public const String CMD_LOAD_FLASH = "LOAD_FLASH";

        public const String CMD_CLOSE_WINDOW = "CLOSEW";                    // Fermer la fenêtre

        public const String CMD_ADD_SELECTEUR = "ADDSELECT";                // Ajouter un sélecteur
        public const String CMD_SUP_SELECTEUR = "SUPSELECT";                // Supprimer un sélecteur
        public const String CMD_EDIT_SELECTEUR = "EDITSELECT";              // Editer un sélecteur

        public const String CMD_MAJ_FORMULE_SELECTED = "MAJFORMULE";        // Mise à jour de la formule
        public const String CMD_CONTEXTMENU = "CMENU";                      // Le menu contextuel

        public const String CMD_MAJVIEW = "MAJV";                           // Mettre à jour la vue
        public const String CMD_LIREJOURNAL = "READJ";                      // Lire le journal des évènements
        public const String CMD_LIRECOUNTER = "READC";                      // Lire les compteurs
        public const String CMD_READINFO = "CMD_READINFO";

        public const String CMD_MENU_ON = "MENUON";                         // Basculer les menus sur on
        public const String CMD_MENU_OFF = "MENUOFF";                         // Basculer les menus sur on
        public const String CMD_MODIF_MAT = "MODIFMAT";                     // Modifier le matériel

        public const String CMD_ADD_NEW_LINE = "AdNLine";                   // Ajouter une nouvelle line
        public const String CMD_DEL_LINE = "DelLine";

        public const String CMD_PREVIOUS = "previous";
        public const String CMD_NEXT = "next";
        public const String CMD_CLICK = "Click";
        public const String CMD_DELETE = "Delete";
    }
}
