package scxmlgen.Modalities;

import scxmlgen.interfaces.IOutput;



public enum Output implements IOutput{
    
    MUTE_MATOS("[action][MUTE_USER][userName][Matos]"),
    MUTE_AMBROSIO("[action][MUTE_USER][userName][Ambrosio]"),
    MUTE_GUSTAVO("[action][MUTE_USER][userName][Gustavo]"),
    SELF_MUTE("[action][SELF_MUTE]"),
    MUTE_MATOS_IMSERVER("[action][MUTE_USER][userName][Matos][guildName][IMServer]"),
    MUTE_MATOS_TOPICOS("[action][MUTE_USER][userName][Matos][guildName][apicultura]"),
    MUTE_AMBROSIO_IMSERVER("[action][MUTE_USER][userName][Ambrosio][guildName][IMServer]"),
    MUTE_AMBROSIO_TOPICOS("[action][MUTE_USER][userName][Ambrosio][guildName][apicultura]"),
    MUTE_GUSTAVO_IMSERVER("[action][MUTE_USER][userName][Gustavo][guildName][IMServer]"),
    MUTE_GUSTAVO_TOPICOS("[action][MUTE_USER][userName][Gustavo][guildName][apicultura]"),
    SELF_MUTE_IMSERVER("[action][SELF_MUTE][guildName][IMServer]"),
    SELF_MUTE_TOPICOS("[action][SELF_MUTE][guildName][apicultura]"),
    
    DEAF_MATOS("[action][DEAF_USER][userName][Matos]"),
    DEAF_AMBROSIO("[action][DEAF_USER][userName][Ambrosio]"),
    DEAF_GUSTAVO("[action][DEAF_USER][userName][Gustavo]"),
    SELF_DEAF("[action][SELF_DEAF]"),
    DEAF_MATOS_IMSERVER("[action][DEAF_USER][userName][Matos][guildName][IMServer]"),
    DEAF_MATOS_TOPICOS("[action][DEAF_USER][userName][Matos][guildName][apicultura]"),
    DEAF_AMBROSIO_IMSERVER("[action][DEAF_USER][userName][Ambrosio][guildName][IMServer]"),
    DEAF_AMBROSIO_TOPICOS("[action][DEAF_USER][userName][Ambrosio][guildName][apicultura]"),
    DEAF_GUSTAVO_IMSERVER("[action][DEAF_USER][userName][Gustavo][guildName][IMServer]"),
    DEAF_GUSTAVO_TOPICOS("[action][DEAF_USER][userName][Gustavo][guildName][apicultura]"),
    SELF_DEAF_IMSERVER("[action][SELF_DEAF][guildName][IMServer]"),
    SELF_DEAF_TOPICOS("[action][SELF_DEAF][guildName][apicultura]"),
    
    KICK_MATOS("[action][REMOVE_USER][userName][Matos]"),
    KICK_AMBROSIO("[action][REMOVE_USER][userName][Ambrosio]"),
    KICK_GUSTAVO("[action][REMOVE_USER][userName][Gustavo]"),
    KICK_MATOS_IMSERVER("[action][REMOVE_USER][userName][Matos][guildName][IMServer]"),
    KICK_MATOS_TOPICOS("[action][REMOVE_USER][userName][Matos][guildName][apicultura]"),
    KICK_AMBROSIO_IMSERVER("[action][REMOVE_USER][userName][Ambrosio][guildName][IMServer]"),
    KICK_AMBROSIO_TOPICOS("[action][REMOVE_USER][userName][Ambrosio][guildName][apicultura]"),
    KICK_GUSTAVO_IMSERVER("[action][REMOVE_USER][userName][Gustavo][guildName][IMServer]"),
    KICK_GUSTAVO_TOPICOS("[action][REMOVE_USER][userName][Gustavo][guildName][apicultura]"),
    
    BAN_MATOS("[action][BAN_USER][userName][Matos]"),
    BAN_AMBROSIO("[action][BAN_USER][userName][Ambrosio]"),
    BAN_GUSTAVO("[action][BAN_USER][userName][Gustavo]"),
    BAN_MATOS_IMSERVER("[action][BAN_USER][userName][Matos][guildName][IMServer]"),
    BAN_MATOS_TOPICOS("[action][BAN_USER][userName][Matos][guildName][apicultura]"),
    BAN_AMBROSIO_IMSERVER("[action][BAN_USER][userName][Ambrosio][guildName][IMServer]"),
    BAN_AMBROSIO_TOPICOS("[action][BAN_USER][userName][Ambrosio][guildName][apicultura]"),
    BAN_GUSTAVO_IMSERVER("[action][BAN_USER][userName][Gustavo][guildName][IMServer]"),
    BAN_GUSTAVO_TOPICOS("[action][BAN_USER][userName][Gustavo][guildName][apicultura]"),
    
    DELETE_MESSAGE_GERAL("[action][DELETE_LAST_MESSAGE][channelName][geral]"),
    DELETE_MESSAGE_TESTE("[action][DELETE_LAST_MESSAGE][channelName][teste]"),
    DELETE_MESSAGE_SUECA("[action][DELETE_LAST_MESSAGE][channelName][sueca]"),
    DELETE_MESSAGE_GERAL_IMSERVER("[action][DELETE_LAST_MESSAGE][channelName][geral][guildName][IMServer]"),
    DELETE_MESSAGE_TESTE_IMSERVER("[action][DELETE_LAST_MESSAGE][channelName][teste][guildName][IMServer]"),
    DELETE_MESSAGE_SUECA_IMSERVER("[action][DELETE_LAST_MESSAGE][channelName][sueca][guildName][IMServer]"),
    DELETE_MESSAGE_GERAL_TOPICOS("[action][DELETE_LAST_MESSAGE][channelName][geral][guildName][apicultura]"),
    DELETE_MESSAGE_TESTE_TOPICOS("[action][DELETE_LAST_MESSAGE][channelName][teste][guildName][apicultura]"),
    ;
    
    
    
    private String event;

    Output(String m) {
        event=m;
    }
    
    public String getEvent(){
        return this.toString();
    }

    public String getEventName(){
        return event;
    }
}