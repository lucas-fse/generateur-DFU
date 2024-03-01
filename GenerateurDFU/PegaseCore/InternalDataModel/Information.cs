using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Windows;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Runtime.Serialization;

namespace JAY.PegaseCore
{
    
    public enum RIType
    {
        RINNONDEFINI = -1,
        RIBOOL = 0,
        RIANA = 1,
        RIVARNUM = 2,
        RIBOOL_DISTANT = 3,
        RIANA_DISTANT = 4,
        RIVARNUM_DISTANT = 5
    }

    /// <summary>
    /// Classe d'écrivant un retour d'information
    /// </summary>
    public class Information
    {
        
        // Variables
        #region Variables

        private XMLCore.XMLProcessing _navInfo;
        private Boolean _isLienAna;
        private String _idEAna;
        private String _designation;
        private Int32 _hauteurEnLignesInformation;
        private String _mnemologique;
        private Int32 _nbDecimales;
        private Single _pointA_X;
        private Single _pointA_Y;
        private Single _pointB_X;
        private Single _pointB_Y;
        private Single _scaling_a;
        private Single _scaling_b;
        private RIType _typeVariable;
        private Int16 _valMax;
        private Int16 _valMin;
        private String _identRetour;
        private Single _tPtA_X;
        private Single _tPtB_X;
        private Byte _CmdSBCRetour;
        private String _commentaireRI;

        #endregion

        // Propriétés
        #region Propriétés
        /// <summary>
        /// Le commentaire lié au retour d'information
        /// </summary>
        public String CommentaireRI
        {
            get
            {
                if (this._commentaireRI == null)
                {
                    this._commentaireRI = "";
                }
                return this._commentaireRI;
            }
            set
            {
                this._commentaireRI = value;
            }
        } // endProperty: CommentaireRI

        /// <summary>
        /// La valeur d'un pt temporaire fixé uniquement lors du changement du type UI
        /// </summary>
        public Single TPtA_X
        {
            get
            {
                return this._tPtA_X;
            }
            set
            {
                this._tPtA_X = value;
            }
        } // endProperty: TPtA_X

        /// <summary>
        /// La valeur d'un pt temporaire fixé uniquement lors du changement du type UI
        /// </summary>
        public Single TPtB_X
        {
            get
            {
                return this._tPtB_X;
            }
            set
            {
                this._tPtB_X = value;
            }
        } // endProperty: TPtB_X

        /// <summary>
        /// L'attribut XML décrivant l'Id EAna lié (ou non si Value = "")
        /// </summary>
        public String IdEAna
        {
            get
            {
                return this._idEAna;
            }
            set
            {
                this._idEAna = value;
            }
        } // endProperty: IdEAna

        /// <summary>
        /// L'entrée Ana lié à ce retour d'information.
        /// = null s'il n'y a pas de lien.
        /// </summary>
        public ESAna EAna
        {
            get
            {
                ESAna Result = null;

                if (this._idEAna != null)
                {
                    if (this._idEAna != "")
                    {
                        String Id = this._idEAna;
                        var QueryEAna = from E in PegaseData.Instance.ModuleT.EAnas
                                        where E.IdESAna.ToString() == Id.Trim()
                                        select E;

                        if (QueryEAna.Count() > 0)
                        {
                            Result = QueryEAna.First();
                        }
                    }
                }
                return Result;
            }
            set
            {
                if (value != null)
                {
                    this._idEAna = value.IdESAna.ToString(); 
                }
                else
                {
                    this._idEAna = null;
                }
            }
        } // endProperty: EAna

        /// <summary>
        /// L'identifiant du retour d'information
        /// </summary>
        public String IdentRetour
        {
            get
            {
                return this._identRetour;
            }
            set
            {
                this._identRetour = value;
            }
        } // endProperty: IdentRetour

        /// <summary>
        /// La désignation du retour d'information
        /// </summary>
        public String Designation
        {
            get
            {
                return this._designation;
            }
            set
            {
                this._designation = value;
            }
        } // endProperty: Designation

        /// <summary>
        /// Le type de la variable
        /// </summary>
        public RIType TypeVariable
        {
            get
            {
                return this._typeVariable;
            }
            set
            {
                this._typeVariable = value;
            }
        } // endProperty: TypeVariable

        /// <summary>
        /// Mnémonique logique du retour d'information (celui utilisé dans les équations)
        /// </summary>
        public String MnemoLogique
        {
            get
            {
                return this._mnemologique;
            }
            set
            {
                this._mnemologique = value;
            }
        } // endProperty: MnemoLogique

        /// <summary>
        /// La pente de l'équation de changement d'unité (y = a.x + b)
        /// </summary>
        public Single Scaling_a
        {
            get
            {
                return this._scaling_a;
            }
            set
            {
                this._scaling_a = value;
            }
        } // endProperty: Scaling_a

        /// <summary>
        /// L'ordonnée à l'origine de l'équation de changement d'unité (y = a.x + b)
        /// </summary>
        public Single Scaling_b
        {
            get
            {
                return this._scaling_b;
            }
            set
            {
                this._scaling_b = value;
            }
        } // endProperty: Scaling_b

        /// <summary>
        /// La valeur minimale de l'information
        /// </summary>
        public Int16 ValMin
        {
            get
            {
                return this._valMin;
            }
            set
            {
                this._valMin = value;
            }
        } // endProperty: ValMin

        /// <summary>
        /// La valeur maximale de l'information
        /// </summary>
        public Int16 ValMax
        {
            get
            {
                return this._valMax;
            }
            set
            {
                this._valMax = value;
            }
        } // endProperty: ValMax

        /// <summary>
        /// Le nombre de décimales pour le retour d'information
        /// </summary>
        public Int32 NbDecimales
        {
            get
            {
                return this._nbDecimales;
            }
            set
            {
                if (value >= 0 && value <=3)
                {
                    this._nbDecimales = value;
                }
            }
        } // endProperty: NbDecimales

        /// <summary>
        /// Hauteur en lignes d'information (1 ou 2)
        /// </summary>
        public Int32 HauteurEnLignesInformation
        {
            get
            {
                return this._hauteurEnLignesInformation;
            }
            set
            {
                this._hauteurEnLignesInformation = value;
            }
        } // endProperty: HauteurEnLignesInformation

        /// <summary>
        /// Ce retour d'information a-t-il un lien avec une entrée Ana ?
        /// </summary>
        public Boolean IsLienAna
        {
            get
            {
                return this._isLienAna;
            }
            set
            {
                this._isLienAna = value;
            }
        } // endProperty: IsLienAna        

        /// <summary>
        /// Si ce retour d'information est lié à une entrée Ana, 
        /// la valeur minimum renvoyée (en courant ou en tension)
        /// </summary>
        public Single LienAnaValMin
        {
            get
            {
                Single Result = 0;
                if (this.EAna != null)
                {
                    Result = this.EAna.LienAnaValMin;
                }

                return Result;
            }
            set
            {
                if (this.EAna != null)
                {
                    this.EAna.LienAnaValMin = value;
                }
            }
        } // endProperty: LienAnaValMin

        /// <summary>
        /// Si ce retour d'information est lié à une entrée Ana, 
        /// la valeur maximum renvoyée (en courant ou en tension)
        /// </summary>
        public Single LienAnaValMax
        {
            get
            {
                Single Result = 0;
                if (this.EAna != null)
                {
                    Result = this.EAna.LienAnaValMax;
                }
                return Result;
            }
            set
            {
                if (this.EAna != null)
                {
                    this.EAna.LienAnaValMax = value;
                }
            }
        } // endProperty: LienAnaValMax

        /// <summary>
        /// Si ce retour d'information est lié à une entrée Ana, 
        /// l'unité de la valeur (en courant ou en tension)
        /// </summary>
        public String LienAnaUnite
        {
            get
            {
                String Result = "";
                if (this.EAna != null)
                {
                    Result = this.EAna.LienAnaUnite;
                }
                return Result;
            }
            set
            {
                if (this.EAna != null)
                {
                    this.EAna.LienAnaUnite = value; 
                }
            }
        } // endProperty: LienAnaUnite

        /// <summary>
        /// Si ce retour d'information est lié à une entrée Ana, 
        /// le type de l'entrée courant (intensité) ou en tension
        /// </summary>
        public String LienAnaType
        {
            get
            {
                String Result = "";
                if (this.EAna != null)
                {
                    Result = this.EAna.LienAnaType;
                }
                return Result;
            }
            set
            {
                if (this.EAna != null)
                {
                    this.EAna.LienAnaType = value;
                }
            }
        } // endProperty: LienAnaType

        /// <summary>
        /// Le nom décrit dans le lien Ana
        /// </summary>
        public String LienAnaName
        {
            get
            {
                String Result = "";
                if (this.EAna != null)
                {
                    Result = this.EAna.LienAnaName;
                }
                return Result;
            }
            set
            {
                if (this.EAna != null)
                {
                    this.EAna.LienAnaName = value;
                }
            }
        } // endProperty: LienAnaName

        /// <summary>
        /// Valeur lue de la calibration, point A
        /// </summary>
        public Single PointA_X
        {
            get
            {
                return this._pointA_X;
            }
            set
            {
                this._pointA_X = value;
            }
        } // endProperty: PointA_X

        /// <summary>
        /// Valeur attendue lors de la calibration, point A
        /// </summary>
        public Single PointA_Y
        {
            get
            {
                return this._pointA_Y;
            }
            set
            {
                this._pointA_Y = value;
            }
        } // endProperty: PointA_Y

        /// <summary>
        /// Valeur lue de la calibration, point B
        /// </summary>
        public Single PointB_X
        {
            get
            {
                return this._pointB_X;
            }
            set
            {
                this._pointB_X = value;
            }
        } // endProperty: PointB_X

        /// <summary>
        /// Valeur attendue lors de la calibration, point A
        /// </summary>
        public Single PointB_Y
        {
            get
            {
                return this._pointB_Y;
            }
            set
            {
                this._pointB_Y = value;
            }
        } // endProperty: PointB_Y

        /// <summary>
        /// L'octet permettant de paramétrer le comportement sur support de charge
        /// </summary>
        public Byte  CmdSBCRetour
        {
            get
            {
                return this._CmdSBCRetour;

            }
            set
            {
                this._CmdSBCRetour = value;
            }
        } // endProperty:  CmdSBCRetour

        /// <summary>
        /// La commande du relais 1 du support de charge
        /// </summary>
        public Boolean CmdRelai1SBC
        {
            get
            {
                return JAY.PegaseCore.Helper.BitHelper.ReadBit(this.CmdSBCRetour, 0);
            }
            set
            {
                this.CmdSBCRetour = JAY.PegaseCore.Helper.BitHelper.SetBit(this.CmdSBCRetour, 0, value);
            }
        } // endProperty: CmdRelai1SBC

        /// <summary>
        /// La commande du relais 2 du support de charge
        /// </summary>
        public Boolean CmdRelai2SBC
        {
            get
            {
                return JAY.PegaseCore.Helper.BitHelper.ReadBit(this.CmdSBCRetour, 1);
            }
            set
            {
                this.CmdSBCRetour = JAY.PegaseCore.Helper.BitHelper.SetBit(this.CmdSBCRetour, 1, value);
            }
        } // endProperty: CmdRelai2SBC

        /// <summary>
        /// La commande du buzzer du support de charge
        /// </summary>
        public Boolean CmdBuzSBC
        {
            get
            {
                return JAY.PegaseCore.Helper.BitHelper.ReadBit(this.CmdSBCRetour, 2);
            }
            set
            {
                this.CmdSBCRetour = JAY.PegaseCore.Helper.BitHelper.SetBit(this.CmdSBCRetour, 2, value);
            }
        } // endProperty: CmdBuzSBC

        /// <summary>
        /// La commande du buzzer du support de charge
        /// </summary>
        public Boolean CmdDistant
        {
            get;
            set;
        } // endProperty: CmdBuzSBC

        #endregion

        // Constructeur
        #region Constructeur

        public Information(XElement information)
        {
            this._navInfo = new XMLCore.XMLProcessing();
            this._navInfo.OpenXML(information);
            this.InitEAna();
            this.InitRIFromXML();
        }

        public Information(String ID)
        {
            this.IdentRetour = ID;
            this.Designation = "Nouveau retour d'information";
            this.HauteurEnLignesInformation = 1;
            this.IsLienAna = false;
            this.MnemoLogique = "";
            this.NbDecimales = 1;
            this.TypeVariable = RIType.RINNONDEFINI;
            this.Scaling_a = 1;
            this.Scaling_b = 0;
            this.PointA_X = 0;
            this.PointA_Y = 0;
            this.PointB_X = 10;
            this.PointB_Y = 10;
            this.ValMax = 10;
            this.ValMin = 0;
            this.TPtA_X = -1;
            this.TPtB_X = -1;
            this.CmdSBCRetour = 0;
        }

        #endregion

        // Méthodes
        #region Méthodes
        
        /// <summary>
        /// Initialiser l'ensemble des données concernant l'entrée Analogique dont ce retour d'information dépend
        /// si il dépend d'une entrée analogique
        /// </summary>
        private void InitEAna ( )
        {
            IEnumerable<XElement> lienEAnas;

            // le noeud existe-t-il ?
            lienEAnas = this._navInfo.GetNodeByPath("LienEAna");
            if (lienEAnas != null)
            {
                XElement lienEAna = lienEAnas.FirstOrDefault();
                if (lienEAna != null && lienEAna.Attribute(XMLCore.XML_ATTRIBUTE.IDEANA) != null && lienEAna.Attribute(XMLCore.XML_ATTRIBUTE.IDEANA).Value != "-1")
                {
                    // le noeud existe, remplir les valeur à partir du xml
                    this.IsLienAna = true;

                    // Définition de l'unité
                    if (lienEAna.Attribute(XMLCore.XML_ATTRIBUTE.UNIT) != null)
                    {
                        this.LienAnaUnite = lienEAna.Attribute(XMLCore.XML_ATTRIBUTE.UNIT).Value; 
                    }
                    else
                    {
                        this.LienAnaUnite = "undefined";
                        //lienEAna.Add(new XAttribute(XMLCore.XML_ATTRIBUTE.UNIT, this.LienAnaUnite));
                    }

                    // Définition du type
                    if (lienEAna.Attribute(XMLCore.XML_ATTRIBUTE.TYPE) != null)
                    {
                        this.LienAnaType = lienEAna.Attribute(XMLCore.XML_ATTRIBUTE.TYPE).Value;
                    }
                    else
                    {
                        this.LienAnaType = "undefined";
                        //lienEAna.Add(this.LienAnaType);
                    }
                    // Définition de la valeur
                    if (lienEAna.Attribute(XMLCore.XML_ATTRIBUTE.VALUE) != null)
                    {
                        this.LienAnaName = lienEAna.Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value;
                    }
                    else
                    {
                        this.LienAnaName = "undefined";
                    }

                    // Définition de la plage de valeur
                    if (lienEAna.Attribute(XMLCore.XML_ATTRIBUTE.PLAGE_VALEUR) != null)
                    {
                        // Chercher la valeur min et max gérée en courant ou en tension
                        String Plage = lienEAna.Attribute(XMLCore.XML_ATTRIBUTE.PLAGE_VALEUR).Value;
                        Char[] separator = new Char[] { '/', '=' };

                        // Nettoyer la chaine et la séparer
                        if (Plage != null)
                        {
                            if (Plage.Length > 0)
                            {
                                if (Plage.Substring(0, 1) == "/")
                                {
                                    Plage = Plage.Substring(1);
                                }
                            }
                        }
                        String[] Parts = Plage.Split(separator);

                        if (Parts.Length == 4)
                        {
                            for (int i = 0; i < 4; i += 2)
                            {
                                Parts[i + 1] = Parts[i + 1].Trim();
                                Parts[i + 1] = XMLCore.Tools.FixFloatStringSeparator(Parts[i + 1]);

                                if (Parts[i].ToLower() == "min")
                                {    
                                    this.LienAnaValMin = Convert.ToSingle(Parts[i + 1]);
                                }
                                else if (Parts[i].ToLower() == "max")
                                {
                                    this.LienAnaValMax = Convert.ToSingle(Parts[i + 1]);
                                }
                            }
                        }
                    }
                    else
                    {
                        this.LienAnaValMin = 0;
                        this.LienAnaValMax = 1;
                    }

                    // Définition de la référence à une entrée Ana
                    if (lienEAna.Attribute(XMLCore.XML_ATTRIBUTE.IDEANA) != null)
                    {
                        this._idEAna = lienEAna.Attribute(XMLCore.XML_ATTRIBUTE.IDEANA).Value;
                    }
                    else
                    {
                        this._idEAna = "0";
                        //lienEAna.Add(this._idEAna);
                    }
                }
            }
            else
            {
                // si le noeud n'existe pas remplir avec les valeurs par défaut
                this.IsLienAna = false;
                this.LienAnaValMax = 0;
                this.LienAnaValMin = 0;
            }
        } // endMethod: InitEAna
        
        /// <summary>
        /// Initialiser les retours d'informations
        /// </summary>
        public void InitRIFromXML ( )
        {
            String SValue;
            // IdentRI
            this._identRetour = this._navInfo.GetNodeByPath("IdentRetour").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value;
            // Designation
            this._designation = this._navInfo.GetNodeByPath("Desigantion").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value;
            // Hauteur
            SValue = this._navInfo.GetNodeByPath("HauteurEnLignesInformation").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value;
            this._hauteurEnLignesInformation = Convert.ToInt32(SValue);
            // Mnémologique
            this._mnemologique = this._navInfo.GetNodeByPath("MnemoLogique").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value;
            // NbDecimales
            SValue = this._navInfo.GetNodeByPath("NbDecimales").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value;
            if (SValue != "")
            {
                this._nbDecimales = Convert.ToInt32(SValue);
            }
            else
            {
                this._nbDecimales = 0;
            }
            // PointA_X
            this._pointA_X = this.GetPointValue("PointA_X");
            // PointA_Y
            this._pointA_Y = this.GetPointValue("PointA_Y");
            // PointB_X
            this._pointB_X = this.GetPointValue("PointB_X");
            // PointB_Y
            this._pointB_Y = this.GetPointValue("PointB_Y");
            // Point Théorique A_X
            this._tPtA_X = this.GetPointValue("tPtA_X");
            // Point Théorique
            this._tPtA_X = this.GetPointValue("tPtB_X");

            // Scaling_a
            SValue = this._navInfo.GetNodeByPath("Scaling_a").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value;
            try
            {
                SValue = JAY.XMLCore.Tools.FixFloatStringSeparator(SValue);
                this._scaling_a = Convert.ToSingle(SValue);
            }
            catch
            {
                this._scaling_a = 1;
            }
            // Scaling_b
            SValue = this._navInfo.GetNodeByPath("Scaling_b").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value;
            SValue = JAY.XMLCore.Tools.FixFloatStringSeparator(SValue);
            try
            {
                SValue = JAY.XMLCore.Tools.FixFloatStringSeparator(SValue);
                this._scaling_b = Convert.ToSingle(SValue);
            }
            catch
            {
                this._scaling_b = 0;
            }
            // Type variable
            try
            {
                this._typeVariable = (RIType)Convert.ToInt32(this._navInfo.GetNodeByPath("TypeVariable").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value);
            }
            catch
            {
                this._typeVariable = RIType.RINNONDEFINI;
            }
            // ValMin
            SValue = this._navInfo.GetNodeByPath("ValMin").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value;
            if (SValue.Length > 0)
            {
                try
                {
                    SValue = JAY.XMLCore.Tools.FixFloatStringSeparator(SValue);
                    this._valMin = Convert.ToInt16(SValue);
                }
                catch (Exception)
                {
                    this._valMin = 0;
                }
            }
            else
            {
                this._valMin = 0;
            }
            // ValMax
            SValue = this._navInfo.GetNodeByPath("ValMax").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value;
            if (SValue.Length > 0)
            {
                try
                {
                    SValue = JAY.XMLCore.Tools.FixFloatStringSeparator(SValue);
                    this._valMax = Convert.ToInt16(SValue);
                }
                catch (Exception)
                {
                    this._valMax = 1;
                }
            }
            else
            {
                this._valMax = this.ValMin;
                this._valMax++;
            }

            // CmdSBCRetour
            String Value = this._navInfo.GetValue("CmdSBCRetour", "", "", XMLCore.XML_ATTRIBUTE.VALUE);
            try
	        {
                this._CmdSBCRetour = Convert.ToByte(Value);
	        }
	        catch
	        {
                this._CmdSBCRetour = 0;
	        }
            // retour distant
            SValue = this._navInfo.GetValue("RetourDistant", "", "", XMLCore.XML_ATTRIBUTE.VALUE);
            try
            {
                this.CmdDistant = Convert.ToBoolean(SValue);
            }
            catch
            {
                this.CmdDistant = false;
            }
        } // endMethod: InitRI
        
        /// <summary>
        /// Sauver les données dans le noeud xml spécifié
        /// </summary>
        public void SaveRI ( XElement node )
        {
            XMLCore.XMLProcessing XProcess = new XMLCore.XMLProcessing();
            XProcess.OpenXML(node);
            // IdentRI
            XProcess.GetNodeByPath("IdentRetour").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value = this._identRetour;
            // Designation
            XProcess.GetNodeByPath("Desigantion").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value = this._designation;
            // Hauteur
            XProcess.GetNodeByPath("HauteurEnLignesInformation").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value = this._hauteurEnLignesInformation.ToString();
            // Mnémologique
            XProcess.GetNodeByPath("MnemoLogique").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value = this._mnemologique;
            // Nb décimales
            XProcess.GetNodeByPath("NbDecimales").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value = this._nbDecimales.ToString();
            // PointA_X
            this.SetPointValue(this._pointA_X, "PointA_X", XProcess);
            // PointA_Y
            this.SetPointValue(this._pointA_Y, "PointA_Y", XProcess);
            // PointB_X
            this.SetPointValue(this._pointB_X, "PointB_X", XProcess);
            // PointB_Y
            this.SetPointValue(this._pointB_Y, "PointB_Y", XProcess);
            // Point théorique A_X
            this.SetPointValue(this._tPtA_X, "tPtA_X", XProcess);
            // Point théorique B_X
            this.SetPointValue(this._tPtB_X, "tPtB_X", XProcess);
            // Scaling_a
            XProcess.GetNodeByPath("Scaling_a").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value = this._scaling_a.ToString();
            // Scaling_b
            XProcess.GetNodeByPath("Scaling_b").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value = this._scaling_b.ToString();
            // Type variable
            XProcess.GetNodeByPath("TypeVariable").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value = ((Int32)this._typeVariable).ToString();
            // ValMin
            XProcess.GetNodeByPath("ValMin").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value = this._valMin.ToString();
            // ValMax
            XProcess.GetNodeByPath("ValMax").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value = this._valMax.ToString();
            // Lien EAna
            if (this.IsLienAna)
            {
                XProcess.GetNodeByPath("LienEAna").First().Attribute(XMLCore.XML_ATTRIBUTE.IDEANA).Value = this.IdEAna.ToString();
                XProcess.SetValue("LienEAna", "", "", XMLCore.XML_ATTRIBUTE.VALUE, this.LienAnaName);
                XProcess.SetValue("LienEAna", "", "", XMLCore.XML_ATTRIBUTE.PLAGE_VALEUR, String.Format("/min={0}/max={1}", this.LienAnaValMin, this.LienAnaValMax));
                XProcess.SetValue("LienEAna", "", "", XMLCore.XML_ATTRIBUTE.UNIT, this.LienAnaUnite);
            }
            else
            {
                XProcess.GetNodeByPath("LienEAna").First().Attribute(XMLCore.XML_ATTRIBUTE.IDEANA).Value = "-1";
            }
            XProcess.GetNodeByPath("CmdSBCRetour").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value = this._CmdSBCRetour.ToString();
            XProcess.GetNodeByPath("RetourDistant").First().Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value = this.CmdDistant.ToString();
        } // endMethod: SaveRI

        /// <summary>
        /// Acquérir la valeur d'un point désignée par name
        /// </summary>
        private Single GetPointValue(String name)
        {
            Single Result;

            // 1 - Vérifier que la variable existe
            IEnumerable<XElement> elements = this._navInfo.GetNodeByPath(name);

            XElement element = null;
            if (elements != null)
            {
                element = elements.FirstOrDefault();
            }

            // 2 - Lire la valeur
            if (element != null)
            {
                String Value = element.Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value;

                Value = XMLCore.Tools.FixFloatStringSeparator(Value);
                Result = Convert.ToSingle(Value);
            }
            else
            {
                this.SetPointValue(0f, name, this._navInfo);
                Result = 0f;
            }

            return Result;
        } // endMethod: GetPointValue

        /// <summary>
        /// Définir la valeur d'un point désigné par value
        /// </summary>
        private void SetPointValue(Single value, String name, XMLCore.XMLProcessing XProcess)
        {
            // 1 - Vérifier que la variable existe
            IEnumerable<XElement> elements = XProcess.GetNodeByPath(name);

            XElement element = null;
            if (elements != null)
            {
                element = elements.FirstOrDefault();
            }
            // 2 - si non, la créer
            if (element == null)
            {
                XElement previousElement = XProcess.GetNodeByPath("HauteurEnLignesInformation").First();
                element = new XElement(previousElement);
                element.Attribute(XMLCore.XML_ATTRIBUTE.CODE).Value = name;
                element.Attribute(XMLCore.XML_ATTRIBUTE.DESCRIPTION).Value = "Info de calibration";
                XProcess.InsertXElement(previousElement, element);
            }

            // 3 - enregistrer la valeur
            element.Attribute(XMLCore.XML_ATTRIBUTE.VALUE).Value = value.ToString();
        } // endMethod: SetPointValue
        
        /// <summary>
        /// Supprimer le retour d'information
        /// </summary>
        public void Remove ( )
        {
            this._navInfo.RootNode.Remove();
        } // endMethod: Remove

        #endregion

        // Messages
        #region Messages

        #endregion

       
    } // endClass: Information
}
