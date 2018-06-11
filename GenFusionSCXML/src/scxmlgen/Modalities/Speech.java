package scxmlgen.Modalities;

import scxmlgen.interfaces.IModality; 

public enum Speech implements IModality{  

    MUTE_MATOS("[action][MUTE_USER][userName][Matos]", 1500),
    MUTE_AMBROSIO("[action][MUTE_USER][userName][Ambrosio]", 1500),
    MUTE_GUSTAVO("[action][MUTE_USER][userName][Gustavo]", 1500),
    SELF_MUTE("[action][SELF_MUTE]", 1500),
    MUTE_MATOS_IMSERVER("[action][MUTE_USER][userName][Matos][guildName][IMServer]", 1500),
    MUTE_MATOS_TOPICOS("[action][MUTE_USER][userName][Matos][guildName][apicultura]", 1500),
    MUTE_AMBROSIO_IMSERVER("[action][MUTE_USER][userName][Ambrosio][guildName][IMServer]", 1500),
    MUTE_AMBROSIO_TOPICOS("[action][MUTE_USER][userName][Ambrosio][guildName][apicultura]", 1500),
    MUTE_GUSTAVO_IMSERVER("[action][MUTE_USER][userName][Gustavo][guildName][IMServer]", 1500),
    MUTE_GUSTAVO_TOPICOS("[action][MUTE_USER][userName][Gustavo][guildName][apicultura]", 1500),
    SELF_MUTE_IMSERVER("[action][SELF_MUTE][guildName][IMServer]", 1500),
    SELF_MUTE_TOPICOS("[action][SELF_MUTE][guildName][apicultura]", 1500),
    
    DEAF_MATOS("[action][DEAF_USER][userName][Matos]", 1500),
    DEAF_AMBROSIO("[action][DEAF_USER][userName][Ambrosio]", 1500),
    DEAF_GUSTAVO("[action][DEAF_USER][userName][Gustavo]", 1500),
    SELF_DEAF("[action][SELF_DEAF]", 1500),
    DEAF_MATOS_IMSERVER("[action][DEAF_USER][userName][Matos][guildName][IMServer]", 1500),
    DEAF_MATOS_TOPICOS("[action][DEAF_USER][userName][Matos][guildName][apicultura]", 1500),
    DEAF_AMBROSIO_IMSERVER("[action][DEAF_USER][userName][Ambrosio][guildName][IMServer]", 1500),
    DEAF_AMBROSIO_TOPICOS("[action][DEAF_USER][userName][Ambrosio][guildName][apicultura]", 1500),
    DEAF_GUSTAVO_IMSERVER("[action][DEAF_USER][userName][Gustavo][guildName][IMServer]", 1500),
    DEAF_GUSTAVO_TOPICOS("[action][DEAF_USER][userName][Gustavo][guildName][apicultura]", 1500),
    SELF_DEAF_IMSERVER("[action][SELF_DEAF][guildName][IMServer]", 1500),
    SELF_DEAF_TOPICOS("[action][SELF_DEAF][guildName][apicultura]", 1500),
    
    USER_MATOS("[userName][Matos]",1500),
    USER_AMBROSIO("[userName][Ambrosio]",1500),
    USER_GUSTAVO("[userName][Gustavo]",1500),
    USER_MATOS_IMSERVER("[userName][Matos][guildName][IMServer]",1500),
    USER_MATOS_TOPICOS("[userName][Matos][guildName][apicultura]",1500),
    USER_AMBROSIO_IMSERVER("[userName][Ambrosio][guildName][IMServer]",1500),
    USER_AMBROSIO_TOPICOS("[userName][Ambrosio][guildName][apicultura]",1500),
    USER_GUSTAVO_IMSERVER("[userName][Gustavo][guildName][IMServer]",1500),
    USER_GUSTAVO_TOPICOS("[userName][Gustavo][guildName][apicultura]",1500),
    
    CHANNEL_GERAL("[channelName][geral]",1500),
    CHANNEL_TESTE("[channelName][teste]",1500),
    CHANNEL_SUECA("[channelName][sueca]",1500),
    CHANNEL_GERAL_IMSERVER("[channelName][geral][guildName][IMServer]",1500),
    CHANNEL_TESTE_IMSERVER("[channelName][teste][guildName][IMServer]",1500),
    CHANNEL_SUECA_IMSERVER("[channelName][sueca][guildName][IMServer]",1500),
    CHANNEL_GERAL_TOPICOS("[channelName][geral][guildName][apicultura]",1500),
    CHANNEL_TESTE_TOPICOS("[channelName][teste][guildName][apicultura]",1500),
    ;


private String event; 
private int timeout;
Speech(String m, int time) {
	event=m;
	timeout=time;
}
@Override
public int getTimeOut(){
	return timeout;
}
@Override
public String getEventName(){
	return event;
}
@Override
public String getEvName(){
	return getModalityName().toLowerCase() +event.toLowerCase();
}

}
