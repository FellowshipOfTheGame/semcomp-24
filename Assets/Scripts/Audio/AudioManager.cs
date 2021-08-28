using UnityEngine.Audio;
using System;
using UnityEngine;

/*
   Lembrando que deve-se referenciar, no código que ser utilizado, o gerenciador de áudios
   para que possa utilizar seus métodos. Como a função de tocar permite que  mandemos uma string
   podemos criar nos scripts, outras strings, que são configuráveis pelo inspetor, onde designam
   os sons que devem ser tocados para cada obstáculo ou objeto, portanto deve-se fazer as alterações
   para que possa integrar sem problemas.
*/



/*
   Gerenciador de áudio, onde será configurado todos os sons do seu jogo
   de acordo com as configurações ditadas no inspetor.
*/
public class AudioManager : MonoBehaviour {

    //vetor da classe de sons, contendo todos os sons e suas configurações
    public Sound[] sounds;
    
    //referência estática para o gerenciador de áudio atual, para verificarmos se existe apenas um, caso haja mais, teremos de os deletar
    public static AudioManager instance;

    void Awake()
    {
       CheckInstance();

       //evita que o gerenciador de som seja destruído ao trocar de cena, para evitar que os sons recomeçem toda vez que trocar de cena
       DontDestroyOnLoad(gameObject);

       AddSource();  
    }

    void Start()
    {
        Play("MainTheme");
    }

    /*
       Função que checa se há apenas um gerenciador de áudio na cena
    */
    void CheckInstance()
    {
        //se não fora achado um gerenciador...
        if(instance == null)
        {
            //esse objeto se torna o gerenciador atual
            instance = this; 
        }
        else //se achar...
        {
            //apaga-se esse objeto por ser uma duplicata e retorna
            Destroy(gameObject);
            return;
        }
    }

    /*
       Função que passa o vetor de sons adicionando as fontes de emissão de cada áudio
       utilizando-se das configurações que configuramos no inspetor.
    */
    void AddSource()
    {
        foreach (Sound sound in sounds)
        {
            //adicionando o componente "AudioSource"
            sound.source = gameObject.AddComponent<AudioSource>();
            
            //adicionando o arquivo de som a ser tocado
            sound.source.clip = sound.clip;

            //e pegando os valores de volume e pitch
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;

            //valor que determina se o som se repetirá ou não
            sound.source.loop = sound.loop;
        }
    }

    /*
       Função que, a partir do nome de um som, procura no vetor de sons pelo som desejado
       e o toca.
    */
    public void Play(string name)
    {
        //queremos achar o som (sound) no vetor de sons (sounds), onde o nome do som é igual ao nome passado para o método
        Sound playSound = Array.Find(sounds, sound => sound.name == name);

        //se o arquivo não existir ou não ser encontrado...
        if(playSound == null)
        {
            //imprime uma mensagem de erro
            Debug.Log("Sound not found.");

            //e retorna
            return;
        }

        //e então tocamos o som que achamos
        playSound.source.Play();
    }
}