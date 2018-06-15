package scxmlgen.Modalities;

import scxmlgen.interfaces.IModality;

/**
 *
 * @author nunof
 */
public enum SecondMod implements IModality{

    MUTE_MATOS("[action][MUTE_USER][userName][Matos]", 2000),
    MUTE_AMBROSIO("[action][MUTE_USER][userName][Ambrosio]", 2000),
    MUTE_GUSTAVO("[action][MUTE_USER][userName][Gustavo]", 2000),
    SELF_MUTE("[action][SELF_MUTE]", 2000),
    MUTE_MATOS_IMSERVER("[action][MUTE_USER][userName][Matos][guildName][IMServer]", 2000),
    MUTE_MATOS_TOPICOS("[action][MUTE_USER][userName][Matos][guildName][apicultura]", 2000),
    MUTE_AMBROSIO_IMSERVER("[action][MUTE_USER][userName][Ambrosio][guildName][IMServer]", 2000),
    MUTE_AMBROSIO_TOPICOS("[action][MUTE_USER][userName][Ambrosio][guildName][apicultura]", 2000),
    MUTE_GUSTAVO_IMSERVER("[action][MUTE_USER][userName][Gustavo][guildName][IMServer]", 2000),
    MUTE_GUSTAVO_TOPICOS("[action][MUTE_USER][userName][Gustavo][guildName][apicultura]", 2000),
    SELF_MUTE_IMSERVER("[action][SELF_MUTE][guildName][IMServer]", 2000),
    SELF_MUTE_TOPICOS("[action][SELF_MUTE][guildName][apicultura]", 2000),
    
    DEAF_MATOS("[action][DEAF_USER][userName][Matos]", 2000),
    DEAF_AMBROSIO("[action][DEAF_USER][userName][Ambrosio]", 2000),
    DEAF_GUSTAVO("[action][DEAF_USER][userName][Gustavo]", 2000),
    SELF_DEAF("[action][SELF_DEAF]", 2000),
    DEAF_MATOS_IMSERVER("[action][DEAF_USER][userName][Matos][guildName][IMServer]", 2000),
    DEAF_MATOS_TOPICOS("[action][DEAF_USER][userName][Matos][guildName][apicultura]", 2000),
    DEAF_AMBROSIO_IMSERVER("[action][DEAF_USER][userName][Ambrosio][guildName][IMServer]", 2000),
    DEAF_AMBROSIO_TOPICOS("[action][DEAF_USER][userName][Ambrosio][guildName][apicultura]", 2000),
    DEAF_GUSTAVO_IMSERVER("[action][DEAF_USER][userName][Gustavo][guildName][IMServer]", 2000),
    DEAF_GUSTAVO_TOPICOS("[action][DEAF_USER][userName][Gustavo][guildName][apicultura]", 2000),
    SELF_DEAF_IMSERVER("[action][SELF_DEAF][guildName][IMServer]", 2000),
    SELF_DEAF_TOPICOS("[action][SELF_DEAF][guildName][apicultura]", 2000),
    
    DEAF_USER("[action][DEAF_USER]", 2000),
    DEAF_USER_IMSERVER("[action][DEAF_USER][guildName][IMServer]", 2000),
    DEAF_USER_TOPICOS("[action][DEAF_USER][guildName][apicultura]", 2000),
    
    MUTE_USER("[action][MUTE_USER]", 2000),
    MUTE_USER_IMSERVER("[action][MUTE_USER][guildName][IMServer]", 2000),
    MUTE_USER_TOPICOS("[action][MUTE_USER][guildName][apicultura]", 2000),
    
    KICK_USER("[action][REMOVE_USER]",2000),
    KICK_USER_IMSERVER("[action][REMOVE_USER][guildName][IMServer]",2000),
    KICK_USER_TOPICOS("[action][REMOVE_USER][guildName][apicultura]",2000),
    
    BAN_USER("[action][BAN_USER]",2000),
    BAN_USER_IMSERVER("[action][BAN_USER][guildName][IMServer]",2000),
    BAN_USER_TOPICOS("[action][BAN_USER][guildName][apicultura]",2000),
    
    DELETE_MESSAGE("[action][DELETE_LAST_MESSAGE]",2000),
    DELETE_MESSAGE_IMSERVER("[action][DELETE_LAST_MESSAGE][guildName][IMServer]",2000),
    DELETE_MESSAGE_TOPICOS("[action][DELETE_LAST_MESSAGE][guildName][apicultura]",2000),
    
    DELETE_MESSAGE_CHANNEL_TEST_IMSERVER("[action][DELETE_LAST_MESSAGE][channelName][teste][guildName][IMServer]",2000),
    DELETE_MESSAGE_CHANNEL_SUECA_IMSERVER("[action][DELETE_LAST_MESSAGE][channelName][sueca][guildName][IMServer]",2000),
    DELETE_MESSAGE_CHANNEL_GERAL_IMSERVER("[action][DELETE_LAST_MESSAGE][channelName][geral][guildName][IMServer]",2000),
    
    KICK_USER_MATOS_IMSERVER("[action][REMOVE_USER][userName][Matos][guildName][IMServer]",2000),
    KICK_USER_AMBROSIO_IMSERVER("[action][REMOVE_USER][userName][Ambrosio][guildName][IMServer]",2000),
    KICK_USER_GUSTAVO_IMSERVER("[action][REMOVE_USER][userName][Gustavo][guildName][IMServer]",2000),

    BAN_USER_MATOS_IMSERVER("[action][BAN_USER][userName][Matos][guildName][IMServer]",2000),
    BAN_USER_AMBROSIO_IMSERVER("[action][BAN_USER][userName][Ambrosio][guildName][IMServer]",2000),
    BAN_USER_GUSTAVO_IMSERVER("[action][BAN_USER][userName][Gustavo][guildName][IMServer]",2000),

    DELETE_MESSAGE_CHANNEL_TEST_TOPICOS("[action][DELETE_LAST_MESSAGE][channelName][teste][guildName][apicultura]",2000),
    DELETE_MESSAGE_CHANNEL_GERAL_TOPICOS("[action][DELETE_LAST_MESSAGE][channelName][geral][guildName][apicultura]",2000),
    
    KICK_USER_MATOS_TOPICOS("[action][REMOVE_USER][userName][Matos][guildName][apicultura]",2000),
    KICK_USER_AMBROSIO_TOPICOS("[action][REMOVE_USER][userName][Ambrosio][guildName][apicultura]",2000),
    KICK_USER_GUSTAVO_TOPICOS("[action][REMOVE_USER][userName][Gustavo][guildName][apicultura]",2000),

    BAN_USER_MATOS_TOPICOS("[action][BAN_USER][userName][Matos][guildName][apicultura]",2000),
    BAN_USER_AMBROSIO_TOPICOS("[action][BAN_USER][userName][Ambrosio][guildName][apicultura]",2000),
    BAN_USER_GUSTAVO_TOPICOS("[action][BAN_USER][userName][Gustavo][guildName][apicultura]",2000),

    
    ;
    
    
    private String event;
    private int timeout;


    SecondMod(String m, int time) {
        event=m;
        timeout=time;
    }

    @Override
    public int getTimeOut() {
        return timeout;
    }

    @Override
    public String getEventName() {
        //return getModalityName()+"."+event;
        return event;
    }

    @Override
    public String getEvName() {
        return getModalityName().toLowerCase()+event.toLowerCase();
    }
    
}
