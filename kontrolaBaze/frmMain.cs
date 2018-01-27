using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient; 
namespace kontrolaBaze
{
    public partial class frmMain : Form
    {

        String connString_ = "";

        public frmMain()
        {
            InitializeComponent();
        }

        private void btn_konekcija_Click(object sender, EventArgs e)
        {
            //idemo redom sta mi treba
            //Server=myServerAddress;Port=1234;Database=myDataBase;Uid=myUsername;Pwd = myPassword;

            if (txt_sreverName.Text=="")
            {
                MessageBox.Show("Morate uneti naziv servera");
                return; 
            }

            if (txt_DatabaseName.Text == "")
            {
                MessageBox.Show("Morate uneti naziv baze");
                return;
            }

            if (txt_userName.Text == "")
            {
                MessageBox.Show("Morate uneti korisnicko ime");
                return;
            }

            if (txt_password.Text == "")
            {
                MessageBox.Show("Morate uneti lozinku");
                return;
            }

            connString_ = "Server=" + txt_sreverName.Text + "; Port=3306; Database=" + txt_DatabaseName.Text + "; Uid=" + txt_userName.Text + "; Pwd=" + txt_password.Text;

            //sada mozemo da idemo dalje

            MySqlConnection conn_ = new MySqlConnection(connString_);
            try
            {
                conn_.Open();
                //ako je uspeo to je ok
                conn_.Close();
                conn_ = null;
                lbl_porukaKonekcija.Text = "Konekcija uspesno izvrsena na bazu: " + txt_DatabaseName.Text ;
                kontrolaToolStripMenuItem.Enabled = true;
            }
            catch
            {
                lbl_porukaKonekcija.Text = "Imate problem sa konekcijom proverite parametre";
                kontrolaToolStripMenuItem.Enabled = false;
            }

        }

        private void jEToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            //ok sada mozemo da idemo query po query
            txtRezultatKontrole.Text = DateTime.Now.ToString() ;

            //ok aj prvo da napravimo dve matrice u jednoj je sql a u 
            //drugoj je komentar odnosno naziv kontrole

            String[] sqlMat = new String[64];
            String[] commentarMat = new String[64];

           
            MySqlConnection con_ = new MySqlConnection(connString_);
            MySqlDataReader reader_;
            

            try
            {
                con_.Open();
            } catch
            {
                MessageBox.Show("Problem sa konkcijom na bazu");
                return;
            }

            pb1.Maximum = 65;

            MySqlCommand com_ = new MySqlCommand("", con_);
            // KONTROLA PAR BAZE
            sqlMat[0] = "select * FROM je_par where cast(brojparc as UNSIGNED)=0 or (cast(podbroj as UNSIGNED)=0 and podbroj<>\"000\") or brojparc is null or podbroj is null or length(brojparc)<>5 or length(podbroj)<>3";
            sqlMat[1] = "select * FROM je_par where brposlis is null or cast(brposlis as UNSIGNED)=0 or LENGTH(brposlis)<>5";
            sqlMat[2] = "SELECT * FROM je_par where (skica=\"\" or skica is null) and (cast(god as unsigned) <> 0) or brojplana is null or brojplana=\"\"";
            sqlMat[3] = "SELECT * FROM (SELECT brojparc, podbroj , ulicapotes , je_par.sulice, \"ulice\" FROM je_par LEFT OUTER JOIN je_ulice on je_par.sulice=je_ulice.sulice where ulicapotes=1 ) as A where sulice is null UNION SELECT * FROM (SELECT brojparc, podbroj , ulicapotes , je_par.sulice, \"potes\" FROM je_par LEFT OUTER JOIN je_potesi on je_par.sulice=je_potesi.spotesa where ulicapotes=9 ) as A where sulice is null";
            sqlMat[4] = "SELECT * from je_par where length(hektari)<>5 and length(ari)<>3 and length(metri)<>3 AND (cast(hektari as UNSIGNED)*10000+cast(ari as UNSIGNED)*100+cast(metri as UNSIGNED) )<=0";
            sqlMat[5] = "SELECT * FROM (SELECT je_par.*,NAZIV FROM je_par LEFT OUTER JOIN kat_gradjevi on je_par.ggradjzem=kat_gradjevi.SIFRA ) A where naziv is null";
            sqlMat[6] = "SELECT * from (SELECT * from je_par LEFT OUTER JOIN kat_kultura on je_par.skulture=kat_kultura.idKulture ) A WHERE nazivkult is null";
            sqlMat[7] = "SELECT * FROM je_par where brstavke=0 or brstavke=\"\" or brstavke is null";

            commentarMat[0] = "001 Oznaka za broj i podbroj parcele.................................Par";
            commentarMat[1] = "002 Oznaka za broj B_lista ..........................................Par ";
            commentarMat[2] = "004 Oznaka za plan, skicu/god. ili manual/god........................Par ";
            commentarMat[3] = "006 Oznaka i {ifra ulice ili potesa..................................Par ";
            commentarMat[4] = "008  Oznaka za povr{inu parcele (hektari,ari,metri)...................Par";
            commentarMat[5] = "010 Oznaka za gra|evinsko zemqi{te...................................Par ";
            commentarMat[6] = "011 [ifra na~ina kori{}ewa zemqi{ta (kultura)........................Par ";
            commentarMat[7] = "013 Oznaka za broj stavke na parceli.................................Par ";      
            
            // KONTROLA KOR BAZE

            sqlMat[8]= "SELECT * from je_kor where brposlis=\"\" or brposlis is null or brposlis=0 or cast(brposlis as UNSIGNED)=0";
            sqlMat[9]= "SELECT * from je_kor where prezime is null or prezime=\"\"";
            sqlMat[10]= "SELECT * from je_kor where mesto is null or mesto=\"\"";
            sqlMat[11]= "SELECT * FROM je_kor where (cast(broj as UNSIGNED)=0 and uzbroj<>\"\") or (cast(broj as UNSIGNED)=0 and LENGTH(broj)>0)";
            sqlMat[12] = "SELECT * FROM je_kor where cast(matbrgra as UNSIGNED)=0 or matbrgra=\"\" or matbrgra is null or LENGTH(matbrgra)>13";
            sqlMat[13] = "SELECT * FROM je_kor where (cast(brojilac as UNSIGNED)>cast(imenilac as UNSIGNED)) and (obimprava<>4 and obimprava<>1)";
            sqlMat[14] = "SELECT * FROM je_kor where obimprava=4 and brojilac=\"0000Z\" and imenilac=\"0000S\"";
            sqlMat[15] = "SELECT * FROM je_kor where obimprava=1 and (brojilac<>\"000001\" or imenilac<>\"000001\")";
            sqlMat[16]= "select * from (SELECT * FROM je_kor LEFT OUTER JOIN kat_svojina on je_kor.ds_ps=kat_svojina.SIFRA ) as A WHERE naziv is null";
            sqlMat[17] = "select * from (SELECT * FROM je_kor LEFT OUTER JOIN kat_pravovrsta on je_kor.vrstaprava=kat_pravovrsta.SIFRA ) A WHERE naziv is null";
            sqlMat[18] = "SELECT brposlis, matbrgra, prezime, ds_ps,vrstaprava FROM je_kor WHERE vrstaprava=5 and ds_ps<>2";
            sqlMat[19] = "select * from (SELECT * FROM je_kor LEFT OUTER JOIN kat_pravoobim on je_kor.obimprava=kat_pravoobim.SIFRA ) A WHERE naziv is null";
            sqlMat[20] = "select * from (SELECT * FROM je_kor LEFT OUTER JOIN kat_nosiocip on je_kor.sifralica=kat_nosiocip.SIFRA ) A WHERE naziv is null";
            sqlMat[21] = "SELECT * FROM je_kor where (cast(brojilac as UNSIGNED)/cast(imenilac as UNSIGNED))=1 AND obimprava<>1";
            sqlMat[22] = "SELECT * FROM je_kor where (cast(brojilac as UNSIGNED)/cast(imenilac as UNSIGNED))<>1 AND obimprava>2";
            sqlMat[23] = "SELECT * FROM je_kor where (cast(brojilac as UNSIGNED)/cast(imenilac as UNSIGNED))=1 AND vrstaprava<>1";
            sqlMat[24] = "SELECT * FROM je_kor where sifralica in (2000,2001) AND (obimprava>2 or ds_ps>1)";

            commentarMat[8] = "015 Oznaka za broj B_lista...........................................Kor ";
            commentarMat[9] = "017  Oznaka za prezime ili naziv pravnog lica.........................Kor";
            commentarMat[10] = "018  Oznaka za mesto stanovawa ili sedi{te pravnog lica...............Kor ";
            commentarMat[11] = "019  Oznaka za ku}ni broj i podbroj...................................Kor ";
            commentarMat[12] = "020 Oznaka za mati~ni broj gra|ana ili pravnog lica..................Kor ";
            commentarMat[13] = "021  Oznaka za udeo (brojilac/imenilac)...............................Kor ";
            commentarMat[14] = "021  Oznaka za udeo (brojilac/imenilac)...............................Kor ";
            commentarMat[15] = "021  Oznaka za udeo (brojilac/imenilac)...............................Kor ";
            commentarMat[16] = "022 Oznaka za vrstu svojine..........................................Kor ";
            commentarMat[17] = "024 Oznaka za vrstu prava............................................Kor ";
            commentarMat[18] = "024 Oznaka za vrstu prava............................................Kor ";
            commentarMat[19] = "025 Oznaka za obim prava.............................................Kor ";
            commentarMat[20] = "026  Oznaka za {ifru lica.............................................Kor ";
            commentarMat[21] = "026  Obim prava.............................................Kor ";
            commentarMat[22] = "026  Obim prava.............................................Kor ";
            commentarMat[23] = "026  Vrsta prava.............................................Kor ";
            commentarMat[24] = "026  DS-PS.............................................Kor ";
            

            // KONTROLA KOR BAZE
            sqlMat[25]= "select * FROM je_vli where cast(brojparc as UNSIGNED)=0 or (cast(podbroj as UNSIGNED)=0 and podbroj<>\"000\") or brojparc is null or podbroj is null or length(brojparc)<>5 or length(podbroj)<>3";
            sqlMat[26] = "select brojparc,podbroj,zk_br, nacinkor,evidencija FROM je_vli where (if(nacinkor=3001 or nacinkor=3002 or nacinkor=3003,1,0)-evidencija)<>0";
            sqlMat[27] = "select * FROM je_vli where zk_br=0 or zk_br is null";
            sqlMat[28] = "select * FROM je_vli where pstatuso=9 and nacinkor not in (\"10040\",\"10050\",\"10049\",\"10059\",\"20103\") ";
            sqlMat[29] = "SELECT * FROM je_vli LEFT OUTER JOIN kat_objektiosnovizg on je_vli.pstatuso = kat_objektiosnovizg.SIFRA where OSNOVIZG is null";
            sqlMat[30] = "SELECT * FROM je_vli WHERE pstatuso<>0 and nacinkor in (\"3001\",\"3002\", \"3003\")";
            sqlMat[31] = "SELECT BROJPARC, PODBROJ, ZK_BR FROM je_vli where NACINKOR is null or NACINKOR not in (SELECT SKULTURE FROM kat_objektikultura)";
            sqlMat[32] = "SELECT * FROM je_vli WHERE povrsina=0 or povrsina is null AND (povrsina>0 and nacinpovr=7)";
            sqlMat[33] = "SELECT * FROM je_vli where (nacinkor=\"3001\" or nacinkor=\"3002\" or nacinkor=\"3003\") and (nacinpovr<>0 or nacinpovr is null)";
            sqlMat[34] = "SELECT * FROM je_vli where (nacinkor<>\"3001\" and nacinkor<>\"3002\" and nacinkor<>\"3003\") and (nacinpovr = \"\" and nacinpovr is null)";
            sqlMat[35] = "SELECT brojparc, podbroj, zk_br FROM je_vli where (svojina1 is null) or (svojina1 not in (select sifra FROM kat_svojina))";
            sqlMat[36] = "SELECT * FROM je_vli where sifralica is null or sifralica=\"\"";
            sqlMat[37] = "SELECT * FROM je_vli where matbrgra is null or matbrgra=\"\"";
            sqlMat[38] = "SELECT * FROM je_vli where matbrgra is null or matbrgra=\"\"";
            sqlMat[39] = "select * FROM (SELECT * FROM (SELECT * FROM je_vli where nacinkor=\"3002\" ) as AA LEFT OUTER JOIN kat_objektiposprostor on AA.zku=kat_objektiposprostor.SIFRA) AB where naziv is null";


            commentarMat[25] = "028 Oznaka za broj i podbroj parcele.................................Vli ";
            commentarMat[26] = "029 Oznaka za objekat ili poseban deo objekta........................Vli ";
            commentarMat[27] = "030 Oznaka za redni broj objekta.....................................Vli ";
            commentarMat[28] = "031 Oznaka za pravni status objekta..................................Vli ";
            commentarMat[29] = "031 Oznaka za pravni status objekta..................................Vli ";
            commentarMat[30] = "031 Oznaka za pravni status objekta..................................Vli ";
            commentarMat[31] = "037 Povr{ina objekta ili posebnog dela objekta ......................Vli ";
            commentarMat[32] = "034 Povr{ina objekta ili posebnog dela objekta ......................Vli ";
            commentarMat[33] = "038 Oznaka za na~in utvr|ivawa korisne povr{ine poseb.dela objekta...Vli ";
            commentarMat[34] = "038 Oznaka za na~in utvr|ivawa korisne povr{ine poseb.dela objekta...Vli ";
            commentarMat[35] = "039 Oznaka za vrstu svojine..........................................Vli ";
            commentarMat[36] = "043 Oznaka za {ifru lica.............................................Vli ";
            commentarMat[37] = "044 Oznaka za mati~ni broj gra|ana ili pravnog lica..................Vli ";
            commentarMat[38] = "045 Oznaka za prezime fizi~kog ili naziv pravnog lica................Vli ";
            commentarMat[39] = "047 Na~in kori{}ewa poslovnog prostora posebnog dela objekta.........Vli ";
            
            // KONTROLA UPARENOSTI BAZA
            sqlMat[40] = "select brojparc,podbroj,count(*) from (select DISTINCT brojparc,podbroj,brposlis,brojplana,sta_krug FROM je_par ) as AA GROUP BY brojparc,podbroj having count(*)>1";
            sqlMat[41] = "SELECT brojparc, podbroj, sulice, broj, uzbroj, count(*) FROM ( SELECT DISTINCT brojparc, podbroj, sulice, broj, uzbroj FROM je_par WHERE ulicapotes = 1 AND BROJ IS NOT NULL ) AS AA GROUP BY brojparc, podbroj, broj HAVING count(*) > 1";
            sqlMat[42] = "select brojparc,podbroj,skulture,count(*) FROM je_par where skulture not in (370,360,361) GROUP BY brojparc,podbroj,skulture having count(*)>1";
            sqlMat[43] = "SELECT DISTINCT brposlis,\"par\" FROM je_par where brposlis not IN (select DISTINCT brposlis FROM je_kor) union SELECT DISTINCT brposlis,\"kor\" FROM je_kor where brposlis not IN (select DISTINCT brposlis FROM je_par)";
            sqlMat[44] = "SELECT DISTINCT * FROM ( SELECT brojparc, podbroj, brstavke, ( cast(hektari AS UNSIGNED) * 10000 + cast(ari AS UNSIGNED) * 100 + cast(metri AS UNSIGNED)) AS P FROM je_par WHERE skulture = 360 ) AA LEFT OUTER JOIN ( SELECT brojparc, podbroj, zk_br, povrsina FROM je_vli ) BB ON AA.brojparc = BB.brojparc AND AA.podbroj = BB.podbroj AND AA.brstavke = BB.zk_br WHERE abs(P - IFNULL(povrsina, 0)) <> 0 UNION SELECT DISTINCT * FROM ( SELECT brojparc, podbroj, zk_br, povrsina FROM je_vli ) BB LEFT OUTER JOIN ( SELECT brojparc, podbroj, brstavke, ( cast(hektari AS UNSIGNED) * 10000 + cast(ari AS UNSIGNED) * 100 + cast(metri AS UNSIGNED)) AS P FROM je_par WHERE skulture = 360 ) AA ON AA.brojparc = BB.brojparc AND AA.podbroj = BB.podbroj AND AA.brstavke = BB.zk_br WHERE abs(P - povrsina) <> 0";
            sqlMat[45] = "SELECT * FROM (SELECT A.brojparc, A.podbroj, je_gli.SIFRA FROM (select brojparc,podbroj from je_par where brposlis in (SELECT brposlis from je_kor where je_kor.vrstaprava=5)) A LEFT OUTER JOIN je_gli on A.brojparc=je_gli.BROJPARC and A.podbroj=je_gli.PODBROJ) B where sifra is null or sifra<>145;";
            sqlMat[46] = "SELECT * FROM (SELECT A.*,je_gli.SIFRA FROM (SELECT distinct brposlis,brojparc,podbroj FROM je_par where skulture=361) A LEFT OUTER JOIN je_gli on A.brojparc=je_gli.BROJPARC and A.podbroj=je_gli.PODBROJ) B WHERE sifra is null or sifra<>147;";
            sqlMat[47] = "SELECT * FROM (SELECT DISTINCT A.*,je_par.skulture FROM (SELECT BROJPARC,PODBROJ, ZK_BR FROM je_gli where SIFRA=147) A LEFT OUTER JOIN je_par on A.BROJPARC=je_par.brojparc and A.PODBROJ=je_par.podbroj AND A.ZK_BR=je_par.brstavke) B where skulture is null or skulture=\"\" or skulture<>361";
            sqlMat[48] = "SELECT brposlis, matbrgra FROM je_kor WHERE MATBRGRA IN ( SELECT MATBRGRA FROM je_kor GROUP BY MATBRGRA, BROJILAC, IMENILAC, brposlis HAVING count(*) > 1 ) ORDER BY brposlis";
            sqlMat[49] = "select brposlis, sum(cast(brojilac as UNSIGNED))/cast(imenilac as UNSIGNED) MJ from je_kor GROUP BY brposlis having MJ<>1";
            sqlMat[50] = "SELECT count(*),matbrgra from (SELECT DISTINCT matbrgra, prezime, imeoca, ime, ulica, mesto FROM je_kor ORDER BY matbrgra) A GROUP BY matbrgra HAVING count(*)>1";
            sqlMat[51] = "SELECT count(*),brposlis from (SELECT DISTINCT brposlis, obimprava FROM je_kor ORDER BY brposlis) A GROUP BY brposlis HAVING count(*)>1";
            sqlMat[52] = "SELECT DISTINCT brposlis, \"nema u kor\" FROM je_par where brposlis not IN (select DISTINCT brposlis FROM je_kor) union SELECT DISTINCT brposlis, \"nema u par\" FROM je_kor where brposlis not IN (select DISTINCT brposlis FROM je_par)";
            sqlMat[53] = "select * from (SELECT je_kor.*, je_lica.matbrgra as mbr1 FROM je_kor LEFT OUTER JOIN je_lica on je_lica.matbrgra=je_kor.matbrgra AND je_lica.prezime=je_kor.prezime and je_lica.sifralica=je_kor.sifralica and je_lica.ime=je_kor.ime and je_lica.imeoca=je_kor.imeoca and je_lica.ulica=je_kor.ulica and je_lica.broj=je_kor.broj and je_lica.uzbroj=je_kor.uzbroj and je_lica.mesto=je_kor.mesto ) A where mbr1 is null or mbr1=\"\"";
            sqlMat[54] = "SELECT brjoparc, podbroj FROM je_vli GROUP BY BROJPARC, PODBROJ, ZK_BR, EVIDENCIJA, MATBRGRA having count(*)>1";
            sqlMat[55] = "SELECT BROJPARC, PODBROJ FROM (SELECT distinct brojparc, PODBROJ, OBIMPRAVA FROM je_vli) aa GROUP BY brojparc, podbroj, obimprava having count(*)>1";
            sqlMat[56] = "SELECT BROJPARC,PODBROJ, OBIMPRAVA, IMENILAC, BROJILAC FROM je_vli where IMENILAC='000001' and BROJILAC='000001' and OBIMPRAVA<>1 union SELECT BROJPARC,PODBROJ, OBIMPRAVA, IMENILAC, BROJILAC FROM  je_vli where IMENILAC<>'000001' and BROJILAC<>'000001' and OBIMPRAVA=1";
            sqlMat[57] = "SELECT BROJPARC,PODBROJ, ZK_BR,  cast(BROJILAC as UNSIGNED) as BROJILAC, cast(IMENILAC as UNSIGNED) as IMENILAC FROM je_vli GROUP BY BROJPARC,PODBROJ, ZK_BR, imenilac having sum(cast(BROJILAC as UNSIGNED))<>cast(IMENILAC as UNSIGNED)";
            sqlMat[58] = "SELECT * from (SELECT * FROM (SELECT DISTINCT brojparc BR,podbroj PB FROM je_gli) A LEFT OUTER JOIN (SELECT distinct BROJPARC as BR1, PODBROJ PB1 FROM je_par ) B on A.BR=B.BR1 and A.PB=B.PB1) C where br1 is null ";
            sqlMat[59] = "SELECT * from (SELECT * FROM (select DISTINCT BROJPARC BR1 ,PODBROJ PB1,ZK_BR ZK1 FROM je_gli where SIFRA=141) A LEFT OUTER JOIN (select DISTINCT brojparc BR2,podbroj PB2,zk_br ZK2 FROM je_vli where pstatuso=5 ) B on A.BR1=B.BR2 and A.PB1=B.PB2 and A.ZK1=B.ZK2) C where Br2 is null;  ";
            sqlMat[60] = "SELECT * from (SELECT * FROM (select DISTINCT BROJPARC BR1 ,PODBROJ PB1,ZK_BR ZK1 FROM je_gli where SIFRA>=141 and SIFRA<=148) A inner JOIN (select DISTINCT brojparc BR2,podbroj PB2,zk_br ZK2 FROM je_vli where pstatuso=9 ) B on A.BR1=B.BR2 and A.PB1=B.PB2 and A.ZK1=B.ZK2) C where Br2 is null ";
            sqlMat[61] = "select * FROM je_gli where (matbrgra is null or  sifralica is null or matbrgra='' or sifralica='') and sifra not in(138,147,141,111,152,153)";
            sqlMat[62] = "SELECT count(*),matbrgra FROM je_lica GROUP BY matbrgra having count(*)>1";
            sqlMat[63] = "SELECT count(*), matbrgra, prezime,imeoca,ime,mesto,adresa,broj,uzbroj FROM je_lica GROUP BY matbrgra, prezime,imeoca,ime,mesto,adresa,broj,uzbroj having count(*)>1";


            commentarMat[40] = "072 Da li su zajedni~ki podaci na A_listu jednoobrazni...............Par ";
            commentarMat[41] = "076 Da li postoje objekti sa dupliranim ku}nim brojem................Par ";
            commentarMat[42] = "077 Da li postoje parcele sa dupliranom kulturom i klasom............Par ";
            commentarMat[43] = "080 Da li sve parcele imaju odgovaraju}i broj B_lista............Par-Kor ";
            commentarMat[44] = "082 Da li se objekti u A_listu sla`u sa objektima u V_listu......Par-Vli ";
            commentarMat[45] = "083 Da li su parcele pod zakupom i delom zgrade upisane u G_list.Par-Gli ";
            commentarMat[46] = "083 Da li su parcele pod zakupom i delom zgrade upisane u G_list.Par-Gli ";
            commentarMat[47] = "083 Da li su parcele pod zakupom i delom zgrade upisane u G_list.Par-Gli ";
            commentarMat[48] = "086 Da li ima dupliranih B_listova...................................Kor";
            commentarMat[49] = "088 Da li su sume udela u B_listovima = 1............................Kor";
            commentarMat[50] = "089 Da li ima dupliranih mati~nih brojeva............................Kor";
            commentarMat[51] = "090 Da li postoje B_listovi sa razli~itim obimom prava...............Kor";
            commentarMat[52] = "092  Da li svi B_listovi imaju odgovaraju}e A_listove.............Kor-Par";
            commentarMat[53] = "094  Da li su svi korisnici iz B_lista uneti u datoteku Lica.....Kor-Lica";
            commentarMat[54] = "095  Da li su zajedni~ki podaci na V2_listu jednoobrazni..............Vli";
            commentarMat[55] = "099  Da li postoje V listovi sa različitim obimom prava po delovima..............Vli";
            commentarMat[56] = "100  Da li su idealni i realni udeli na v listu korektno upisani..............Vli";
            commentarMat[57] = "101  Da li su sume udela u v listu = 1 ..............Vli";
            commentarMat[58] = "109 Da li svi G_listovi imaju odgovaraju}e A_listove.............Gli-Par";
            commentarMat[59] = "111  Da li tereti na objektima odgovaraju pravnom statusu.........Gli-Vl";
            commentarMat[60] = "111  Da li tereti na objektima odgovaraju pravnom statusu.........Gli-Vl";
            commentarMat[61] = "112  Da li su nosioci tereta iz G_lista uneti u datoteku Lica....Gli-Lica";
            commentarMat[62] = "117 Da li ima dupliranih mati~nih brojeva...........................Lica";
            commentarMat[63] = "118 Da li ima dupliranih indikacija.................................Lica";

            
           
           Int32  i;

            for (i=0; i<64; i++)
            {
                try
                {
                    con_.Open();
                }
                catch (Exception ex)
                {
                }

                com_.CommandText = sqlMat[i];
                txtRezultatKontrole.Text=txtRezultatKontrole.Text + System.Environment.NewLine + commentarMat[i] + System.Environment.NewLine;
                int brojac_ = 0;
                try
                {
                    reader_ = com_.ExecuteReader(CommandBehavior.CloseConnection);

                    if (reader_.HasRows)
                    {
                        //sada treba videti sta se stampa
                        while (reader_.Read())
                        {
                            try
                            {
                                txtRezultatKontrole.Text = txtRezultatKontrole.Text + reader_.GetName(0) + "=" + reader_.GetValue(0) + reader_.GetName(1) + "=" + reader_.GetValue(1) + reader_.GetName(2) + "=" + reader_.GetValue(2) + System.Environment.NewLine;
                            }
                            catch
                            {
                                txtRezultatKontrole.Text = txtRezultatKontrole.Text + reader_.GetName(0) + "=" + reader_.GetValue(0) + reader_.GetName(1) + "=" + reader_.GetValue(1) ;
                            }
                            brojac_ += 1;
                           if (brojac_>500)
                            {
                                txtRezultatKontrole.Text = txtRezultatKontrole.Text + System.Environment.NewLine + "Sistematski problem preko 500 gresaka" + System.Environment.NewLine;
                                break;
                            }
                        }
                        txtRezultatKontrole.Text = txtRezultatKontrole.Text + System.Environment.NewLine + com_.CommandText + System.Environment.NewLine;
                    }
                    else
                    {
                        //znaci prosli
                        txtRezultatKontrole.Text = txtRezultatKontrole.Text + "    nema gresaka" + System.Environment.NewLine;
                    }
                    reader_.Close();

                }
                catch (Exception ex)
                {
                   
                    txtRezultatKontrole.Text = txtRezultatKontrole.Text + ex.Message;
                   
                }

                
                pb1.Value = i;
            }

            txtRezultatKontrole.Text = txtRezultatKontrole.Text + System.Environment.NewLine + DateTime.Now.ToString();
            MessageBox.Show("Kraj kontrole");
            pb1.Value = 0;

        }

        private void izvestajUTxtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sf_diag.FileName = "";
            sf_diag.ShowDialog();

            if (sf_diag.FileName == "")
            {
                MessageBox.Show("Morate izabrati file");
                return;
            }
            String[] lines = { txtRezultatKontrole.Text };
            
            System.IO.File.WriteAllLines(sf_diag.FileName, lines);


        }

        private void jEUFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //ok sada mozemo da idemo query po query
            txtRezultatKontrole.Text = DateTime.Now.ToString();

            //ok aj prvo da napravimo dve matrice u jednoj je sql a u 
            //drugoj je komentar odnosno naziv kontrole

            sf_diag.FileName="";
            sf_diag.ShowDialog();

            if (sf_diag.FileName=="")
            {
                Close();
            }

            

            String[] sqlMat = new String[64];
            String[] commentarMat = new String[64];


            MySqlConnection con_ = new MySqlConnection(connString_);
            MySqlDataReader reader_;


            try
            {
                con_.Open();
            }
            catch
            {
                MessageBox.Show("Problem sa konkcijom na bazu");
                return;
            }

            pb1.Maximum = 65;

            MySqlCommand com_ = new MySqlCommand("", con_);
            // KONTROLA PAR BAZE
            sqlMat[0] = "select * FROM je_par where cast(brojparc as UNSIGNED)=0 or (cast(podbroj as UNSIGNED)=0 and podbroj<>\"000\") or brojparc is null or podbroj is null or length(brojparc)<>5 or length(podbroj)<>3";
            sqlMat[1] = "select * FROM je_par where brposlis is null or cast(brposlis as UNSIGNED)=0 or LENGTH(brposlis)<>5";
            sqlMat[2] = "SELECT * FROM je_par where (skica=\"\" or skica is null) and (cast(god as unsigned) <> 0) or brojplana is null or brojplana=\"\"";
            sqlMat[3] = "SELECT * FROM (SELECT brojparc, podbroj , ulicapotes , je_par.sulice, \"ulice\" FROM je_par LEFT OUTER JOIN je_ulice on je_par.sulice=je_ulice.sulice where ulicapotes=1 ) as A where sulice is null UNION SELECT * FROM (SELECT brojparc, podbroj , ulicapotes , je_par.sulice, \"potes\" FROM je_par LEFT OUTER JOIN je_potesi on je_par.sulice=je_potesi.spotesa where ulicapotes=9 ) as A where sulice is null";
            sqlMat[4] = "SELECT * from je_par where length(hektari)<>5 and length(ari)<>3 and length(metri)<>3 AND (cast(hektari as UNSIGNED)*10000+cast(ari as UNSIGNED)*100+cast(metri as UNSIGNED) )<=0";
            sqlMat[5] = "SELECT * FROM (SELECT je_par.*,NAZIV FROM je_par LEFT OUTER JOIN kat_gradjevi on je_par.ggradjzem=kat_gradjevi.SIFRA ) A where naziv is null";
            sqlMat[6] = "SELECT * from (SELECT * from je_par LEFT OUTER JOIN kat_kultura on je_par.skulture=kat_kultura.idKulture ) A WHERE nazivkult is null";
            sqlMat[7] = "SELECT * FROM je_par where brstavke=0 or brstavke=\"\" or brstavke is null";

            commentarMat[0] = "001 Oznaka za broj i podbroj parcele.................................Par";
            commentarMat[1] = "002 Oznaka za broj B_lista ..........................................Par ";
            commentarMat[2] = "004 Oznaka za plan, skicu/god. ili manual/god........................Par ";
            commentarMat[3] = "006 Oznaka i {ifra ulice ili potesa..................................Par ";
            commentarMat[4] = "008  Oznaka za povr{inu parcele (hektari,ari,metri)...................Par";
            commentarMat[5] = "010 Oznaka za gra|evinsko zemqi{te...................................Par ";
            commentarMat[6] = "011 [ifra na~ina kori{}ewa zemqi{ta (kultura)........................Par ";
            commentarMat[7] = "013 Oznaka za broj stavke na parceli.................................Par ";

            // KONTROLA KOR BAZE

            sqlMat[8] = "SELECT * from je_kor where brposlis=\"\" or brposlis is null or brposlis=0 or cast(brposlis as UNSIGNED)=0";
            sqlMat[9] = "SELECT * from je_kor where prezime is null or prezime=\"\"";
            sqlMat[10] = "SELECT * from je_kor where mesto is null or mesto=\"\"";
            sqlMat[11] = "SELECT * FROM je_kor where (cast(broj as UNSIGNED)=0 and uzbroj<>\"\") or (cast(broj as UNSIGNED)=0 and LENGTH(broj)>0)";
            sqlMat[12] = "SELECT * FROM je_kor where cast(matbrgra as UNSIGNED)=0 or matbrgra=\"\" or matbrgra is null or LENGTH(matbrgra)>13";
            sqlMat[13] = "SELECT * FROM je_kor where (cast(brojilac as UNSIGNED)>cast(imenilac as UNSIGNED)) and (obimprava<>4 and obimprava<>1)";
            sqlMat[14] = "SELECT * FROM je_kor where obimprava=4 and brojilac=\"0000Z\" and imenilac=\"0000S\"";
            sqlMat[15] = "SELECT * FROM je_kor where obimprava=1 and (brojilac<>\"000001\" or imenilac<>\"000001\")";
            sqlMat[16] = "select * from (SELECT * FROM je_kor LEFT OUTER JOIN kat_svojina on je_kor.ds_ps=kat_svojina.SIFRA ) as A WHERE naziv is null";
            sqlMat[17] = "select * from (SELECT * FROM je_kor LEFT OUTER JOIN kat_pravovrsta on je_kor.vrstaprava=kat_pravovrsta.SIFRA ) A WHERE naziv is null";
            sqlMat[18] = "SELECT brposlis, matbrgra, prezime, ds_ps,vrstaprava FROM je_kor WHERE vrstaprava=5 and ds_ps<>2";
            sqlMat[19] = "select * from (SELECT * FROM je_kor LEFT OUTER JOIN kat_pravoobim on je_kor.obimprava=kat_pravoobim.SIFRA ) A WHERE naziv is null";
            sqlMat[20] = "select * from (SELECT * FROM je_kor LEFT OUTER JOIN kat_nosiocip on je_kor.sifralica=kat_nosiocip.SIFRA ) A WHERE naziv is null";
            sqlMat[21] = "SELECT * FROM je_kor where (cast(brojilac as UNSIGNED)/cast(imenilac as UNSIGNED))=1 AND obimprava<>1";
            sqlMat[22] = "SELECT * FROM je_kor where (cast(brojilac as UNSIGNED)/cast(imenilac as UNSIGNED))<>1 AND obimprava>2";
            sqlMat[23] = "SELECT * FROM je_kor where (cast(brojilac as UNSIGNED)/cast(imenilac as UNSIGNED))=1 AND vrstaprava<>1";
            sqlMat[24] = "SELECT * FROM je_kor where sifralica in (2000,2001) AND (obimprava>2 or ds_ps>1)";

            commentarMat[8] = "015 Oznaka za broj B_lista...........................................Kor ";
            commentarMat[9] = "017  Oznaka za prezime ili naziv pravnog lica.........................Kor";
            commentarMat[10] = "018  Oznaka za mesto stanovawa ili sedi{te pravnog lica...............Kor ";
            commentarMat[11] = "019  Oznaka za ku}ni broj i podbroj...................................Kor ";
            commentarMat[12] = "020 Oznaka za mati~ni broj gra|ana ili pravnog lica..................Kor ";
            commentarMat[13] = "021  Oznaka za udeo (brojilac/imenilac)...............................Kor ";
            commentarMat[14] = "021  Oznaka za udeo (brojilac/imenilac)...............................Kor ";
            commentarMat[15] = "021  Oznaka za udeo (brojilac/imenilac)...............................Kor ";
            commentarMat[16] = "022 Oznaka za vrstu svojine..........................................Kor ";
            commentarMat[17] = "024 Oznaka za vrstu prava............................................Kor ";
            commentarMat[18] = "024 Oznaka za vrstu prava............................................Kor ";
            commentarMat[19] = "025 Oznaka za obim prava.............................................Kor ";
            commentarMat[20] = "026  Oznaka za {ifru lica.............................................Kor ";
            commentarMat[21] = "026  Obim prava.............................................Kor ";
            commentarMat[22] = "026  Obim prava.............................................Kor ";
            commentarMat[23] = "026  Vrsta prava.............................................Kor ";
            commentarMat[24] = "026  DS-PS.............................................Kor ";


            // KONTROLA KOR BAZE
            sqlMat[25] = "select * FROM je_vli where cast(brojparc as UNSIGNED)=0 or (cast(podbroj as UNSIGNED)=0 and podbroj<>\"000\") or brojparc is null or podbroj is null or length(brojparc)<>5 or length(podbroj)<>3";
            sqlMat[26] = "select brojparc,podbroj,zk_br, nacinkor,evidencija FROM je_vli where (if(nacinkor=3001 or nacinkor=3002 or nacinkor=3003,1,0)-evidencija)<>0";
            sqlMat[27] = "select * FROM je_vli where zk_br=0 or zk_br is null";
            sqlMat[28] = "select * FROM je_vli where pstatuso=9 and nacinkor not in (\"10040\",\"10050\",\"10049\",\"10059\",\"20103\") ";
            sqlMat[29] = "SELECT * FROM je_vli LEFT OUTER JOIN kat_objektiosnovizg on je_vli.pstatuso = kat_objektiosnovizg.SIFRA where OSNOVIZG is null";
            sqlMat[30] = "SELECT * FROM je_vli WHERE pstatuso<>0 and nacinkor in (\"3001\",\"3002\", \"3003\")";
            sqlMat[31] = "SELECT BROJPARC,STRUKTURA,NACINKOR FROM je_vli WHERE (NACINKOR NOT IN(3001, 3002, 3003)) AND((STRUKTURA <> '') AND (STRUKTURA IS NOT NULL))";
            sqlMat[32] = "SELECT * FROM je_vli WHERE povrsina=0 or povrsina is null AND (povrsina>0 and nacinpovr=7)";
            sqlMat[33] = "SELECT BROJPARC,NACINKOR,NACINPOVR FROM je_vli WHERE (NACINKOR not IN (3001, 3002, 3003)) AND NACINPOVR<>0 ORDER BY BROJPARC";
            sqlMat[34] = "select brojparc, brojilac, imenilac, OBIMPRAVA FROM je_vli WHERE instr(BROJILAC,'Z')<>0 and OBIMPRAVA<>4 UNION select brojparc, brojilac, imenilac, OBIMPRAVA FROM je_vli WHERE BROJILAC='000001' AND IMENILAC='000001' and OBIMPRAVA<>1 UNION select brojparc, brojilac, imenilac, OBIMPRAVA FROM je_vli WHERE IMENILAC<>'000001' and OBIMPRAVA=1 UNION select brojparc, brojilac, imenilac, OBIMPRAVA FROM je_vli WHERE BROJILAC>IMENILAC";
            sqlMat[35] = "SELECT brojparc, podbroj, zk_br FROM je_vli where (svojina1 is null) or (svojina1 not in (select sifra FROM kat_svojina))";
            sqlMat[36] = "SELECT * FROM je_vli where sifralica is null or sifralica=\"\"";
            sqlMat[37] = "SELECT * FROM je_vli where matbrgra is null or matbrgra=\"\"";
            sqlMat[38] = "SELECT * FROM je_vli where matbrgra is null or matbrgra=\"\"";
            sqlMat[39] = "select * FROM (SELECT * FROM (SELECT * FROM je_vli where nacinkor=\"3002\" ) as AA LEFT OUTER JOIN kat_objektiposprostor on AA.zku=kat_objektiposprostor.SIFRA) AB where naziv is null";


            commentarMat[25] = "028 Oznaka za broj i podbroj parcele.................................Vli ";
            commentarMat[26] = "029 Oznaka za objekat ili poseban deo objekta........................Vli ";
            commentarMat[27] = "030 Oznaka za redni broj objekta.....................................Vli ";
            commentarMat[28] = "031 Oznaka za pravni status objekta..................................Vli ";
            commentarMat[29] = "031 Oznaka za pravni status objekta..................................Vli ";
            commentarMat[30] = "031 Oznaka za pravni status objekta..................................Vli ";
            commentarMat[31] = "037 Povr{ina objekta ili posebnog dela objekta ......................Vli ";
            commentarMat[32] = "034 Povr{ina objekta ili posebnog dela objekta ......................Vli ";
            commentarMat[33] = "038 Oznaka za na~in utvr|ivawa korisne povr{ine poseb.dela objekta...Vli ";
            commentarMat[34] = "042 Oznaka za udeo (brojilac/imenilac)...............................Vli ";
            commentarMat[35] = "039 Oznaka za vrstu svojine..........................................Vli ";
            commentarMat[36] = "043 Oznaka za {ifru lica.............................................Vli ";
            commentarMat[37] = "044 Oznaka za mati~ni broj gra|ana ili pravnog lica..................Vli ";
            commentarMat[38] = "045 Oznaka za prezime fizi~kog ili naziv pravnog lica................Vli ";
            commentarMat[39] = "047 Na~in kori{}ewa poslovnog prostora posebnog dela objekta.........Vli ";

            // KONTROLA UPARENOSTI BAZA
            sqlMat[40] = "select brojparc,podbroj,count(*) from (select DISTINCT brojparc,podbroj,brposlis,brojplana,sta_krug FROM je_par ) as AA GROUP BY brojparc,podbroj having count(*)>1";
            sqlMat[41] = "SELECT BROJPARC, PODBROJ, JE_PAR.SULICE, je_par.BROJ, je_par.UZBROJ FROM je_par INNER JOIN (SELECT sulice, broj, uzbroj FROM (SELECT DISTINCT brojparc, podbroj, sulice, broj, uzbroj FROM je_par WHERE ulicapotes = 1 AND BROJ >0 ) AS AA GROUP BY SULICE, BROJ, UZBROJ  HAVING count(*) > 1 ) gg ON je_par.sulice=gg.sulice AND je_par.broj = gg.broj AND je_par.uzbroj = gg.uzbroj ORDER BY JE_PAR.SULICE, je_par.BROJ, je_par.UZBROJ";
            sqlMat[42] = "select brojparc,podbroj,skulture,count(*) FROM je_par where skulture not in (370,360,361) GROUP BY brojparc,podbroj,skulture having count(*)>1";
            sqlMat[43] = "SELECT DISTINCT brposlis,\"par\" FROM je_par where brposlis not IN (select DISTINCT brposlis FROM je_kor) union SELECT DISTINCT brposlis,\"kor\" FROM je_kor where brposlis not IN (select DISTINCT brposlis FROM je_par)";
            sqlMat[44] = "SELECT je_par.brojparc,je_par.podbroj, je_par.brstavke, je_par.hektari, je_par.ari, je_par.metri, POVRSINA, skulture FROM je_par LEFT OUTER JOIN je_vli on je_par.brojparc=je_vli.BROJPARC and  je_par.podbroj=je_vli.PODBROJ and je_par.brstavke=je_vli.ZK_BR where skulture=360 AND (hektari*10000+ari*100+metri)<>POVRSINA UNION SELECT  je_par.brojparc,je_par.podbroj, je_par.brstavke, je_par.hektari, je_par.ari, je_par.metri, POVRSINA, skulture FROM je_par LEFT OUTER JOIN je_vli on je_par.brojparc=je_vli.BROJPARC and je_par.podbroj=je_vli.PODBROJ and je_par.brstavke=je_vli.ZK_BR where skulture=360 and povrsina is null";
            sqlMat[45] = "SELECT * FROM (SELECT A.brojparc, A.podbroj, je_gli.SIFRA FROM (select brojparc,podbroj from je_par where brposlis in (SELECT brposlis from je_kor where je_kor.vrstaprava=5)) A LEFT OUTER JOIN je_gli on A.brojparc=je_gli.BROJPARC and A.podbroj=je_gli.PODBROJ) B where sifra is null or sifra<>145;";
            sqlMat[46] = "SELECT * FROM (SELECT A.*,je_gli.SIFRA FROM (SELECT distinct brposlis,brojparc,podbroj FROM je_par where skulture=361) A LEFT OUTER JOIN je_gli on A.brojparc=je_gli.BROJPARC and A.podbroj=je_gli.PODBROJ) B WHERE sifra is null or sifra<>147;";
            sqlMat[47] = "SELECT * FROM (SELECT DISTINCT A.*,je_par.skulture FROM (SELECT BROJPARC,PODBROJ, ZK_BR FROM je_gli where SIFRA=147) A LEFT OUTER JOIN je_par on A.BROJPARC=je_par.brojparc and A.PODBROJ=je_par.podbroj AND A.ZK_BR=je_par.brstavke) B where skulture is null or skulture=\"\" or skulture<>361";
            sqlMat[48] = "SELECT brposlis, matbrgra FROM je_kor WHERE MATBRGRA IN ( SELECT MATBRGRA FROM je_kor GROUP BY MATBRGRA, BROJILAC, IMENILAC, brposlis HAVING count(*) > 1 ) ORDER BY brposlis";
            sqlMat[49] = "select brposlis, sum(cast(brojilac as UNSIGNED))/cast(imenilac as UNSIGNED) MJ from je_kor GROUP BY brposlis having MJ<>1";
            sqlMat[50] = "SELECT count(*),matbrgra from (SELECT DISTINCT matbrgra, prezime, imeoca, ime, ulica, mesto FROM je_kor ORDER BY matbrgra) A GROUP BY matbrgra HAVING count(*)>1";
            sqlMat[51] = "SELECT count(*),brposlis from (SELECT DISTINCT brposlis, obimprava FROM je_kor ORDER BY brposlis) A GROUP BY brposlis HAVING count(*)>1";
            sqlMat[52] = "SELECT DISTINCT brposlis, \"nema u kor\" FROM je_par where brposlis not IN (select DISTINCT brposlis FROM je_kor) union SELECT DISTINCT brposlis, \"nema u par\" FROM je_kor where brposlis not IN (select DISTINCT brposlis FROM je_par)";
            sqlMat[53] = "select * from (SELECT je_kor.*, je_lica.matbrgra as mbr1 FROM je_kor LEFT OUTER JOIN je_lica on je_lica.matbrgra=je_kor.matbrgra AND je_lica.prezime=je_kor.prezime and je_lica.sifralica=je_kor.sifralica and je_lica.ime=je_kor.ime and je_lica.imeoca=je_kor.imeoca and je_lica.ulica=je_kor.ulica and je_lica.broj=je_kor.broj and je_lica.uzbroj=je_kor.uzbroj and je_lica.mesto=je_kor.mesto ) A where mbr1 is null or mbr1=\"\"";
            sqlMat[54] = "SELECT brjoparc, podbroj FROM je_vli GROUP BY BROJPARC, PODBROJ, ZK_BR, EVIDENCIJA, MATBRGRA having count(*)>1";
            sqlMat[55] = "SELECT BROJPARC, PODBROJ FROM (SELECT distinct brojparc, PODBROJ, OBIMPRAVA FROM je_vli) aa GROUP BY brojparc, podbroj, obimprava having count(*)>1";
            sqlMat[56] = "SELECT BROJPARC,PODBROJ, OBIMPRAVA, IMENILAC, BROJILAC FROM je_vli where IMENILAC='000001' and BROJILAC='000001' and OBIMPRAVA<>1 union SELECT BROJPARC,PODBROJ, OBIMPRAVA, IMENILAC, BROJILAC FROM  je_vli where IMENILAC<>'000001' and BROJILAC<>'000001' and OBIMPRAVA=1";
            sqlMat[57] = "SELECT BROJPARC,PODBROJ, ZK_BR,  cast(BROJILAC as UNSIGNED) as BROJILAC, cast(IMENILAC as UNSIGNED) as IMENILAC FROM je_vli GROUP BY BROJPARC,PODBROJ, ZK_BR, imenilac having sum(cast(BROJILAC as UNSIGNED))<>cast(IMENILAC as UNSIGNED)";
            sqlMat[58] = "SELECT * from (SELECT * FROM (SELECT DISTINCT brojparc BR,podbroj PB FROM je_gli) A LEFT OUTER JOIN (SELECT distinct BROJPARC as BR1, PODBROJ PB1 FROM je_par ) B on A.BR=B.BR1 and A.PB=B.PB1) C where br1 is null ";
            sqlMat[59] = "SELECT * from (SELECT * FROM (select DISTINCT BROJPARC BR1 ,PODBROJ PB1,ZK_BR ZK1 FROM je_gli where SIFRA=141) A LEFT OUTER JOIN (select DISTINCT brojparc BR2,podbroj PB2,zk_br ZK2 FROM je_vli where pstatuso=5 ) B on A.BR1=B.BR2 and A.PB1=B.PB2 and A.ZK1=B.ZK2) C where Br2 is null;  ";
            sqlMat[60] = "SELECT * from (SELECT * FROM (select DISTINCT BROJPARC BR1 ,PODBROJ PB1,ZK_BR ZK1 FROM je_gli where SIFRA>=141 and SIFRA<=148) A inner JOIN (select DISTINCT brojparc BR2,podbroj PB2,zk_br ZK2 FROM je_vli where pstatuso=9 ) B on A.BR1=B.BR2 and A.PB1=B.PB2 and A.ZK1=B.ZK2) C where Br2 is null ";
            sqlMat[61] = "select * FROM je_gli where (matbrgra is null or  sifralica is null or matbrgra='' or sifralica='') and sifra not in(138,147,141,111,152,153)";
            sqlMat[62] = "SELECT count(*),matbrgra FROM je_lica GROUP BY matbrgra having count(*)>1";
            sqlMat[63] = "SELECT count(*), matbrgra, prezime,imeoca,ime,mesto,adresa,broj,uzbroj FROM je_lica GROUP BY matbrgra, prezime,imeoca,ime,mesto,adresa,broj,uzbroj having count(*)>1";

            commentarMat[40] = "072 Da li su zajedni~ki podaci na A_listu jednoobrazni...............Par ";
            commentarMat[41] = "076 Da li postoje objekti sa dupliranim ku}nim brojem................Par ";
            commentarMat[42] = "077 Da li postoje parcele sa dupliranom kulturom i klasom............Par ";
            commentarMat[43] = "080 Da li sve parcele imaju odgovaraju}i broj B_lista............Par-Kor ";
            commentarMat[44] = "082 Da li se objekti u A_listu sla`u sa objektima u V_listu......Par-Vli ";
            commentarMat[45] = "083 Da li su parcele pod zakupom i delom zgrade upisane u G_list.Par-Gli ";
            commentarMat[46] = "083 Da li su parcele pod zakupom i delom zgrade upisane u G_list.Par-Gli ";
            commentarMat[47] = "083 Da li su parcele pod zakupom i delom zgrade upisane u G_list.Par-Gli ";
            commentarMat[48] = "086 Da li ima dupliranih B_listova...................................Kor";
            commentarMat[49] = "088 Da li su sume udela u B_listovima = 1............................Kor";
            commentarMat[50] = "089 Da li ima dupliranih mati~nih brojeva............................Kor";
            commentarMat[51] = "090 Da li postoje B_listovi sa razli~itim obimom prava...............Kor";
            commentarMat[52] = "092  Da li svi B_listovi imaju odgovaraju}e A_listove.............Kor-Par";
            commentarMat[53] = "094  Da li su svi korisnici iz B_lista uneti u datoteku Lica.....Kor-Lica";
            commentarMat[54] = "095  Da li su zajedni~ki podaci na V2_listu jednoobrazni..............Vli";
            commentarMat[55] = "099  Da li postoje V listovi sa različitim obimom prava po delovima..............Vli";
            commentarMat[56] = "100  Da li su idealni i realni udeli na v listu korektno upisani..............Vli";
            commentarMat[57] = "101  Da li su sume udela u v listu = 1 ..............Vli";
            commentarMat[58] = "109 Da li svi G_listovi imaju odgovaraju}e A_listove.............Gli-Par";
            commentarMat[59] = "111  Da li tereti na objektima odgovaraju pravnom statusu.........Gli-Vl";
            commentarMat[60] = "111  Da li tereti na objektima odgovaraju pravnom statusu.........Gli-Vl";
            commentarMat[61] = "112  Da li su nosioci tereta iz G_lista uneti u datoteku Lica....Gli-Lica";
            commentarMat[62] = "117 Da li ima dupliranih mati~nih brojeva...........................Lica";
            commentarMat[63] = "118 Da li ima dupliranih indikacija.................................Lica";

            //ono sto smo propustili
            sqlMat[64] = "SELECT BROJPARC, PODBROJ, ZK_BR FROM je_vli where NACINKOR in (3001,3002,3003) AND (ETAZA1<>0 or ETAZA2<>0 or ETAZA3<>0 or ETAZA4<>0)";
            sqlMat[65] = "SELECT * FROM je_vli where BROJSTANA<>0 and EVIDENCIJA=0 ORDER BY BROJPARC, PODBROJ";

            commentarMat[64] = "032 Oznaka za broj eta`a objekta.....................................Vli";
            commentarMat[65] = "036 Oznaka za spratnost posebnog dela objekta .......................Vli";

            Int32 i;

            com_.CommandText = "update je_vli set ETAZA1 = '', ETAZA2 = '', ETAZA3 = '', ETAZA4 = '' where NACINKOR in (3001, 3002, 3003) AND(ETAZA1 <> 0 or ETAZA2 <> 0 or ETAZA3 <> 0 or ETAZA4 <> 0)";
            com_.ExecuteNonQuery();
            com_.CommandText = "update je_vli set evidencija=1 where BROJSTANA<>0 and EVIDENCIJA=0";
            com_.ExecuteNonQuery();
            com_.CommandText = "update je_vli set OBIMPRAVA=4 where instr(BROJILAC,'Z')<>4";
            com_.ExecuteNonQuery();
            com_.CommandText = "UPDATE JE_VLI SET OBIMPRAVA=1 WHERE BROJILAC='000001' AND IMENILAC='000001";
            com_.ExecuteNonQuery();

            using (System.IO.StreamWriter file= new System.IO.StreamWriter(sf_diag.FileName))
            {

            
            for (i = 0; i < 66; i++)
            {
                try
                {
                    con_.Open();
                }
                catch (Exception ex)
                {
                }

                com_.CommandText = sqlMat[i];
                txtRezultatKontrole.Text = commentarMat[i] + System.Environment.NewLine +  txtRezultatKontrole.Text +   System.Environment.NewLine;
                file.WriteLine(System.Environment.NewLine + commentarMat[i] + System.Environment.NewLine);

                int brojac_ = 0;

                try
                {
                    reader_ = com_.ExecuteReader(CommandBehavior.CloseConnection);

                    if (reader_.HasRows)
                    {
                        //sada treba videti sta se stampa
                        while (reader_.Read())
                        {
                            try
                            {
                                file.WriteLine(reader_.GetName(0) + "=" + reader_.GetValue(0) + reader_.GetName(1) + "=" + reader_.GetValue(1) + reader_.GetName(2) + "=" + reader_.GetValue(2) + System.Environment.NewLine);
                            }
                            catch
                            {
                                    file.WriteLine(reader_.GetName(0) + "=" + reader_.GetValue(0) + reader_.GetName(1) + "=" + reader_.GetValue(1));
                            }
                            brojac_ += 1;
                            if (brojac_ > 500)
                            {
                                    file.WriteLine(System.Environment.NewLine + "Sistematski problem preko 500 gresaka" + System.Environment.NewLine);
                                break;
                            }
                        }
                            file.WriteLine(System.Environment.NewLine + com_.CommandText + System.Environment.NewLine);
                    }
                    else
                    {
                            //znaci prosli
                            file.WriteLine("    nema gresaka" + System.Environment.NewLine);
                    }
                    reader_.Close();

                }
                catch (Exception ex)
                {

                        file.WriteLine(ex.Message);

                }


                pb1.Value = i;
            }

                file.WriteLine(System.Environment.NewLine + DateTime.Now.ToString());

            }

            txtRezultatKontrole.Text = DateTime.Now.ToString() + System.Environment.NewLine  + txtRezultatKontrole.Text;

            MessageBox.Show("Kraj kontrole");
            pb1.Value = 0;

        }
    }
}
