using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HiGuys : MonoBehaviour {
    public bool     VirarHb = true;
    public bool     ImIsCamera;                         /*CHECAR SE O SCRIPT ESTÁ NA CAMÊRA*/
    public Sprite   imagemPadrao;
    public Sprite   imagemNato;                         /*IMAGEM ATUAL DO BOTÃO (BOTÃO)*/
    public Sprite   [] define = new Sprite[7];          /*Imagens (CÂMERA CONTROLA), array pra simplificar*/
    public int      CartasViradasHabilitadas = 0;       /*Nome explica por si só (AMBOS)*/
    public int      [] CartasViradasHabilitadasID;      
    public int      [] ImagensViradasHabilitadasID;     
    public int      CartaAtualID;                       /*Identificador da cartar atual*/
    public int      ImagemAtualID;                      /*Identificador da imagem no botão atual*/
    private bool    ImChecked = false;                  /*CHECAGEM SE A CARTA ESTÁ VIRADA OU NÃO (BOTÃO)*/
    private GameObject[] ObjetosButtons;                /*Controlador dos objetos(Botões)*/
    public Text     CreditosManager;                    /*Dennys Status Bar - xD - colorido (Orgulho Gay :D)*/

    public bool RestartI;                               /*Verifica se o script está no botão de reiniciar o jogo*/

    private GameObject FinalWin;                        /*Objetos ao final do jogo*/
    private GameObject FinalWin2;                       /*Objetos ao final do jogo*/
    private GameObject FinalWin3;                       /*Objetos ao final do jogo*/

    private AudioSource Som1;

    public Text valorJogadas, valorAcertos, valorErros, valorTotal;
    void BreakingBread() /*Função para virar e desvirar as cartas*/
    {
        if (VirarHb)/*Pode virar?*/
        {
            ImChecked = !ImChecked; /*true = virada, false = desvirada*/
            GameObject Control = GameObject.FindGameObjectWithTag("MainCamera");
            int CartasHabilitadasViradas = Control.GetComponent<HiGuys>().CartasViradasHabilitadas;
            if (ImChecked)
            {
                GetComponent<Button>().image.sprite = imagemNato;    /*Definir a imagem do botão*/
                CartasHabilitadasViradas += 1; /*número atual +1*/
            }
            else
            {
                GetComponent<Button>().image.sprite = imagemPadrao;         /*Remove a imagem do botão*/
                CartasHabilitadasViradas -= 1; /*número atual -1*/
            }
            Control.GetComponent<HiGuys>().ImagensViradasHabilitadasID[CartasHabilitadasViradas] = ImagemAtualID;
            Control.GetComponent<HiGuys>().CartasViradasHabilitadasID[CartasHabilitadasViradas] = CartaAtualID;
            Control.GetComponent<HiGuys>().CartasViradasHabilitadas = CartasHabilitadasViradas;

            PlayerPrefs.SetInt("Total", PlayerPrefs.GetInt("Total") + 1);
        }
    }

    List<int> numbersAloop;
    List<int> numbersAloop2;

    public void BotaoDesfocado()
    {
        EventSystem.current.SetSelectedGameObject(null); //Créditos: Fórum Unity
    }

    void Start()
    {
        if (ImIsCamera) /*GERANDO PARES ALEATÓRIAS PARAS AS CARTAS E DEFININDO-AS COM AS IMAGENS*/
        {
            Som1 = GetComponent<AudioSource>();
            Som1.enabled = false; /*Para não reproduzir o som automaticamente ao entrar na unity*/
            ObjetosButtons = GameObject.FindGameObjectsWithTag("ButtonOn"); /*Pegar todos os botões de uma só vez!!!*/
            CartasViradasHabilitadasID = new int[3]; /*Instanciando o array*/
            ImagensViradasHabilitadasID = new int[3];

            GetComponent<Animator>().enabled = false;
            FinalWin = GameObject.FindGameObjectWithTag("Finish");
            FinalWin2 = GameObject.FindGameObjectWithTag("Finish2");
            FinalWin3 = GameObject.FindGameObjectWithTag("Finish3");

            FinalWin.SetActive (false);
            FinalWin2.SetActive(false);
            FinalWin3.SetActive(false);

            numbersAloop = new List<int>(); /*PAR 1*/
            numbersAloop2 = new List<int>(); /*PAR 2*/

            PlayerPrefs.DeleteAll(); /*Deletar todas as variaveis armazenadas no registro do nosso jogo*/
            PlayerPrefs.SetInt("Jogadas", 0); 
            PlayerPrefs.SetInt("Acertos", 0);
            PlayerPrefs.SetInt("Erros", 0);
            PlayerPrefs.SetInt("Total", 0);

            while (true)
            {
                int selectRandom = UnityEngine.Random.Range(0, ObjetosButtons.Length / 2);/*Número aleatório ao par 1*/
                int selectRandom2 = UnityEngine.Random.Range(0, ObjetosButtons.Length / 2);/*Número aleatório ao par 2*/

                while (numbersAloop.Contains(selectRandom))                             /*Não repetir os números*/
                    selectRandom = UnityEngine.Random.Range(0, ObjetosButtons.Length / 2);/*Gera outro número random*/

                while (numbersAloop2.Contains(selectRandom2))
                    selectRandom2 = UnityEngine.Random.Range(0, ObjetosButtons.Length / 2);

                /*(GameObject[]).Length retorna a quantidade de objetos no array, 
                 * no caso a quantidade botões encontrados com a tag.*/

                numbersAloop.Add(selectRandom);     /*Adiciona à coleção*/
                numbersAloop2.Add(selectRandom2);

                /*Se a quantidade de itens adicionados à lista par1 e par2, for 
                 maior ou igual a quantidade de (botões/2), sai do loop*/
                if (numbersAloop.Count >= ObjetosButtons.Length / 2 && numbersAloop.Count >= ObjetosButtons.Length / 2)
                    break;/*break = sai do loop ou da condição atual. Usado no switch, for, while, foreach, etc.*/
            }
            for (int i = 0, bt = 0, idatual = 0; bt < ObjetosButtons.Length; bt++)
            {
                /*%2 = resto da divisão | usado no caso para verificar se o número restante é ímpar ou par*/
                /*COLEÇÃO 1 E COLEÇÃO 2 | PAR 1 E PAR 2 (numbersAloop e numbersAloop2)*/

                if (bt % 2 == 0)
                    idatual = numbersAloop[i];
                else
                {
                    idatual = numbersAloop2[i];
                    i++;
                }

                ObjetosButtons[bt].GetComponent<HiGuys>().imagemNato = define[idatual];
                ObjetosButtons[bt].GetComponent<HiGuys>().CartaAtualID = bt;
                ObjetosButtons[bt].GetComponent<HiGuys>().ImagemAtualID = idatual;

                /*DEFINIR A IMAGEM DO BOTÃO ATUAL*/
            }
            StartCoroutine(AlterarCoresFundo());
            StartCoroutine(Creditos("Dennys Barreto"));
        }
        else
        {
            if (!RestartI)
            {
                GetComponent<Button>().onClick.AddListener(BreakingBread); /*Evento do click*/
                imagemPadrao = GetComponent<Button>().image.sprite;
            }
            else
                GetComponent<Button>().onClick.AddListener(Restart);
        }
    }

    enum CoresAlterar
    {
        Normal =1,
        Branco =2,
        Preto =3,
        Verde =4,
        AzulForte=5,
        AzulFraco=6,
        Vermelho=7,
        Cinza=8,
        Rosa=9,
        Rosinha=10,
        CinzaEscuro=11,
        Laranja=12,
        Amarelo=13,
        VerdeEscuro=14
    }
    string AlterarCor(string texto, CoresAlterar cor) /*Cores em HEX*/
    {
        /*Site para pegar cores: http://www.color-hex.com/color */
        string corFinal = string.Empty;
        if (cor == CoresAlterar.Normal)
            return texto.ToString(); //Texto normal, sem alteração de cor.

        switch(cor)
        {
            case CoresAlterar.Branco:
                corFinal = "<color=#ffffff>";
                break;
            case CoresAlterar.Preto:
                corFinal = "<color=#000000>";
                break;
            case CoresAlterar.Vermelho:
                corFinal = "<color=#FF0000>";
                break;
            case CoresAlterar.Verde:
                corFinal = "<color=#00c117>";
                break;

            case CoresAlterar.AzulForte:
                corFinal = "<color=#0038ff>";
                break;

            case CoresAlterar.AzulFraco:
                corFinal = "<color=#00c6f0>";
                break;

            case CoresAlterar.Cinza:
                corFinal = "<color=#787878>";
                break;

            case CoresAlterar.Rosa:
                corFinal = "<color=#d703ff>";
                break;

            case CoresAlterar.CinzaEscuro:
                corFinal = "<color=#ff6700>";
                break;

            case CoresAlterar.Rosinha:
                corFinal = "<color=#ff0382>";
                break;

            case CoresAlterar.Amarelo:
                corFinal = "<color=#fff400>";
                break;

            case CoresAlterar.VerdeEscuro:
                corFinal = "<color=#23a933>";
                break;

            case CoresAlterar.Laranja:
                corFinal = "<color=#ff6700>";
                break;

            default:
                return texto.ToString();
        }
        return corFinal + texto.ToString() + "</color>";
    }
    IEnumerator Creditos(string texto)
    {
        string textoF1, textoF2;
        textoF1 = "Dennys";
        textoF2 = "Barreto";

        while (true)
        {
            CreditosManager.text = string.Empty;

            for (int i = 0; i < texto.Length; i++)
            {
                CreditosManager.text += AlterarCor(texto[i].ToString(), (CoresAlterar)new System.Random().Next(1, 14));
                yield return new WaitForSeconds(0.2f);
            }

            bool volteSempre = false;
            for (int i = 0; i < 4; i++)
            {
                int cor1, cor2;
                cor1 = UnityEngine.Random.Range(1, 14);
                cor2 = UnityEngine.Random.Range(1, 14);

            volte:
                CreditosManager.text = string.Empty;
                for (int x = 0; x < (textoF1.Length + textoF2.Length); x++)
                {
                    if (x < textoF1.Length)
                        CreditosManager.text += AlterarCor(textoF1[x].ToString(), (CoresAlterar)cor1);
                    else
                    {
                        if (x == textoF1.Length)
                            CreditosManager.text += AlterarCor(" ", CoresAlterar.Normal);
                        CreditosManager.text += AlterarCor(textoF2[x - textoF1.Length].ToString(), (CoresAlterar)cor2);
                    }
                }

                yield return new WaitForSeconds(0.3f);
                if (!volteSempre)
                {
                    int tempCor1, tempCor2;
                    tempCor1 = cor1;
                    tempCor2 = cor2;

                    cor1 = tempCor2;
                    cor2 = tempCor1;
                    volteSempre = !volteSempre;
                    goto volte;
                }
                volteSempre = false;
            }
        }        
    }
	
    IEnumerator DesvirarCarta(GameObject BOTAOMANAGER1, GameObject BOTAOMANAGER2)
    {
        CartasViradasHabilitadas = 0;

        Sprite imgPadrao1 = BOTAOMANAGER1.GetComponent<HiGuys>().imagemPadrao;
        Sprite imgPadrao2 = BOTAOMANAGER2.GetComponent<HiGuys>().imagemPadrao;
        yield return new WaitForSeconds(0.5f);

        Som1.enabled = true;
        Som1.Play();
        BOTAOMANAGER1.GetComponent<Button>().image.sprite = imgPadrao1;   /*Definir uma imagem nula*/
        BOTAOMANAGER2.GetComponent<Button>().image.sprite = imgPadrao2;

        BOTAOMANAGER1.GetComponent<HiGuys>().ImChecked = false;     /*Alterar os status das booleanas no script*/
        BOTAOMANAGER2.GetComponent<HiGuys>().ImChecked = false;

        BOTAOMANAGER1.GetComponent<HiGuys>().VirarHb = true;
        BOTAOMANAGER2.GetComponent<HiGuys>().VirarHb = true;        /*Permitir que o usuário clique no botão*/
        yield return 0;
    }

    struct RGBCores
    {
        public float r;
        public float g;
        public float b;
        /*Semelhante a um Vector3 (float x, float y, float z)*/
    }

    void Restart()
    {
        /*Recarrega a cena atual*/
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    IEnumerator AlterarCoresFundo() /*EFEITOS VISUAIS BASEADOS NA UNITY*/
    {
        RGBCores RGBCoresFundo;
        RGBCoresFundo.r = 0f;
        RGBCoresFundo.g = 0.5f;
        RGBCoresFundo.b = 1.0f;
        /*{0f, 0.5f, 1.0f} - Cores de início*/

        float AtualizacaoTempo = 0.02f;
        Camera CameraManager = GetComponent<Camera>();

        for (; ;) /*Loop infinito, mesma coisa que um while(true)*/
        {
            /*Valores RGB, com base nos valores mostrados na própria Unity(programa)*/                          /*R                 G               B               A*/
            for (; RGBCoresFundo.g < 1.0f; RGBCoresFundo.g += 0.005f, CameraManager.backgroundColor = new Color(RGBCoresFundo.r, RGBCoresFundo.g, RGBCoresFundo.b, 0f))
                yield return new WaitForSeconds(AtualizacaoTempo);
            for (; RGBCoresFundo.b < 1.0f; RGBCoresFundo.g += 0.005f, CameraManager.backgroundColor = new Color(RGBCoresFundo.r, RGBCoresFundo.g, RGBCoresFundo.b, 0f))
                yield return new WaitForSeconds(AtualizacaoTempo);
            for (; RGBCoresFundo.b > 0.01f; RGBCoresFundo.b -= 0.005f, CameraManager.backgroundColor = new Color(RGBCoresFundo.r, RGBCoresFundo.g, RGBCoresFundo.b, 0f))
                yield return new WaitForSeconds(AtualizacaoTempo);
            for (; RGBCoresFundo.r < 1.0f; RGBCoresFundo.r += 0.005f, CameraManager.backgroundColor = new Color(RGBCoresFundo.r, RGBCoresFundo.g, RGBCoresFundo.b, 0f))
                yield return new WaitForSeconds(AtualizacaoTempo);
            for (; RGBCoresFundo.g > 0.01f; RGBCoresFundo.g -= 0.005f, CameraManager.backgroundColor = new Color(RGBCoresFundo.r, RGBCoresFundo.g, RGBCoresFundo.b, 0f))
                yield return new WaitForSeconds(AtualizacaoTempo);
            for (; RGBCoresFundo.b < 1.0f; RGBCoresFundo.b += 0.005f, CameraManager.backgroundColor = new Color(RGBCoresFundo.r, RGBCoresFundo.g, RGBCoresFundo.b, 0f))
                yield return new WaitForSeconds(AtualizacaoTempo);
            for (; RGBCoresFundo.r > 0.01f; RGBCoresFundo.r -= 0.005f, CameraManager.backgroundColor = new Color(RGBCoresFundo.r, RGBCoresFundo.g, RGBCoresFundo.b, 0f))
                yield return new WaitForSeconds(AtualizacaoTempo);
            for (; RGBCoresFundo.g < 0.5f; RGBCoresFundo.g += 0.005f, CameraManager.backgroundColor = new Color(RGBCoresFundo.r, RGBCoresFundo.g, RGBCoresFundo.b, 0f))
                yield return new WaitForSeconds(AtualizacaoTempo);
        }
    }


    IEnumerator ParteFinal() /*GambiarraNation*/
    {
        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < ObjetosButtons.Length; i++)
            GameObject.Destroy(ObjetosButtons[i]);

        GameObject[] Estatisticas = GameObject.FindGameObjectsWithTag("Estatisticas");
        /*Remover as estatísticas -> Total, Acertos, Erros, etc etc*/
        for (int i = 0; i < Estatisticas.Length; i++)
            GameObject.Destroy(Estatisticas[i]);

        GetComponent<Animator>().enabled = true;
        GetComponent<Animator>().SetBool("Animacao", true);
        FinalWin.SetActive(true);
        FinalWin.GetComponent<Text>().enabled = true;
        FinalWin.GetComponent<Text>().text = "Parabéns, você é um lixo.";
        FinalWin.GetComponent<Animator>().SetBool("Animacao", true);

        yield return new WaitForSeconds(2);

        FinalWin2.SetActive(true);
        FinalWin2.GetComponent<Image>().enabled = true;

        FinalWin3.SetActive(true);
        FinalWin3.GetComponent<Text>().enabled = true;
        FinalWin.GetComponent<Text>().text = "Parabéns, você é um mito!";
        yield return null;
    }
	void Update () {
        if (ImIsCamera)
        {
            if(CartasViradasHabilitadas >= 2)
            {
                PlayerPrefs.SetInt("Jogadas", PlayerPrefs.GetInt("Jogadas") + 1);
                ObjetosButtons[CartasViradasHabilitadasID[1]].GetComponent<HiGuys>().VirarHb = false; /*Não pode virar cartas*/
                ObjetosButtons[CartasViradasHabilitadasID[2]].GetComponent<HiGuys>().VirarHb = false;

                if (ImagensViradasHabilitadasID[1] != ImagensViradasHabilitadasID[2])
                {
                    StartCoroutine(DesvirarCarta(ObjetosButtons[CartasViradasHabilitadasID[1]], ObjetosButtons[CartasViradasHabilitadasID[2]]));
                    PlayerPrefs.SetInt("Erros", PlayerPrefs.GetInt("Erros") + 1);
                    return; 
                    /*Não necessário chamar Update(), visto que o Update() é chamado automaticamente a cada frame.*/
                }

                print("Acertou mizeravi");

                PlayerPrefs.SetInt("Acertos", PlayerPrefs.GetInt("Acertos") + 1);
                CartasViradasHabilitadas = 0;
                if(PlayerPrefs.GetInt("Acertos") == (ObjetosButtons.Length/2)) /*Ganhou!!!*/
                {
                    StopCoroutine(AlterarCoresFundo());

                    StartCoroutine(ParteFinal());
                }
            }
            
            valorJogadas.text   = PlayerPrefs.GetInt("Jogadas").ToString();
            valorAcertos.text   = PlayerPrefs.GetInt("Acertos").ToString();
            valorErros.text     = PlayerPrefs.GetInt("Erros").ToString();
            valorTotal.text     = PlayerPrefs.GetInt("Total").ToString();
        }
	}
}