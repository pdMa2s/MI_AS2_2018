package scxmlgen.Modalities;

import scxmlgen.interfaces.IOutput;



public enum Output implements IOutput{
    
    MUTE_MATOS("[action][MUTE_USER][userName][Matos]"),
    MUTE_AMBROSIO("[action][MUTE_USER][userName][Ambrosio]"),
    MUTE_GUSTAVO("[action][MUTE_USER][userName][Gustavo]"),
    SELF_MUTE("[action][SELF_MUTE]"),
    MUTE_MATOS_IMSERVER("[action][MUTE_USER][userName][Matos][guildName][IMServer]"),
    MUTE_MATOS_TOPICOS("[action][MUTE_USER][userName][Matos][guildName][Topicos de Apicultura]"),
    MUTE_AMBROSIO_IMSERVER("[action][MUTE_USER][userName][Ambrosio][guildName][IMServer]"),
    MUTE_AMBROSIO_TOPICOS("[action][MUTE_USER][userName][Ambrosio][guildName][Topicos de Apicultura]"),
    MUTE_GUSTAVO_IMSERVER("[action][MUTE_USER][userName][Gustavo][guildName][IMServer]"),
    MUTE_GUSTAVO_TOPICOS("[action][MUTE_USER][userName][Gustavo][guildName][Topicos de Apicultura]"),
    SELF_MUTE_IMSERVER("[action][SELF_MUTE][guildName][IMServer]"),
    SELF_MUTE_TOPICOS("[action][SELF_MUTE][guildName][Topicos de Apicultura]"),
    
    DEAF_MATOS("[action][DEAF_USER][userName][Matos]"),
    DEAF_AMBROSIO("[action][DEAF_USER][userName][Ambrosio]"),
    DEAF_GUSTAVO("[action][DEAF_USER][userName][Gustavo]"),
    SELF_DEAF("[action][SELF_DEAF]"),
    DEAF_MATOS_IMSERVER("[action][DEAF_USER][userName][Matos][guildName][IMServer]"),
    DEAF_MATOS_TOPICOS("[action][DEAF_USER][userName][Matos][guildName][Topicos de Apicultura]"),
    DEAF_AMBROSIO_IMSERVER("[action][DEAF_USER][userName][Ambrosio][guildName][IMServer]"),
    DEAF_AMBROSIO_TOPICOS("[action][DEAF_USER][userName][Ambrosio][guildName][Topicos de Apicultura]"),
    DEAF_GUSTAVO_IMSERVER("[action][DEAF_USER][userName][Gustavo][guildName][IMServer]"),
    DEAF_GUSTAVO_TOPICOS("[action][DEAF_USER][userName][Gustavo][guildName][Topicos de Apicultura]"),
    SELF_DEAF_IMSERVER("[action][SELF_DEAF][guildName][IMServer]"),
    SELF_DEAF_TOPICOS("[action][SELF_DEAF][guildName][Topicos de Apicultura]"),
    
    KICK_MATOS("[action][REMOVE_USER][userName][Matos]"),
    KICK_AMBROSIO("[action][REMOVE_USER][userName][Ambrosio]"),
    KICK_GUSTAVO("[action][REMOVE_USER][userName][Gustavo]"),
    KICK_MATOS_IMSERVER("[action][REMOVE_USER][userName][Matos][guildName][IMServer]"),
    KICK_MATOS_TOPICOS("[action][REMOVE_USER][userName][Matos][guildName][Topicos de Apicultura]"),
    KICK_AMBROSIO_IMSERVER("[action][REMOVE_USER][userName][Ambrosio][guildName][IMServer]"),
    KICK_AMBROSIO_TOPICOS("[action][REMOVE_USER][userName][Ambrosio][guildName][Topicos de Apicultura]"),
    KICK_GUSTAVO_IMSERVER("[action][REMOVE_USER][userName][Gustavo][guildName][IMServer]"),
    KICK_GUSTAVO_TOPICOS("[action][REMOVE_USER][userName][Gustavo][guildName][Topicos de Apicultura]"),
    KICK_MATOS_REASON_1("[action][REMOVE_USER][userName][Matos][reason][já me esta a irritar]"),
    KICK_MATOS_REASON_2("[action][REMOVE_USER][userName][Matos][reason][não quero mais ouvi-lo]"),
    KICK_AMBROSIO_REASON_1("[action][REMOVE_USER][userName][Ambrosio][reason][já me esta a irritar]"),
    KICK_AMBROSIO_REASON_2("[action][REMOVE_USER][userName][Ambrosio][reason][não quero mais ouvi-lo]"),
    KICK_GUSTAVO_REASON_1("[action][REMOVE_USER][userName][Gustavo][reason][já me esta a irritar]"),
    KICK_GUSTAVO_REASON_2("[action][REMOVE_USER][userName][Gustavo][reason][não quero mais ouvi-lo]"),
    KICK_MATOS_IMSERVER_REASON_1("[action][REMOVE_USER][userName][Matos][guildName][IMServer][reason][já me esta a irritar]"),
    KICK_MATOS_IMSERVER_REASON_2("[action][REMOVE_USER][userName][Matos][guildName][IMServer][reason][não quero mais ouvi-lo]"),
    KICK_MATOS_TOPICOS_REASON_1("[action][REMOVE_USER][userName][Matos][guildName][Topicos de Apicultura][reason][já me esta a irritar]"),
    KICK_MATOS_TOPICOS_REASON_2("[action][REMOVE_USER][userName][Matos][guildName][Topicos de Apicultura][reason][não quero mais ouvi-lo]"),
    KICK_AMBROSIO_IMSERVER_REASON_1("[action][REMOVE_USER][userName][Ambrosio][guildName][IMServer][reason][já me esta a irritar]"),
    KICK_AMBROSIO_IMSERVER_REASON_2("[action][REMOVE_USER][userName][Ambrosio][guildName][IMServer][reason][não quero mais ouvi-lo]"),
    KICK_AMBROSIO_TOPICOS_REASON_1("[action][REMOVE_USER][userName][Ambrosio][guildName][Topicos de Apicultura][reason][já me esta a irritar]"),
    KICK_AMBROSIO_TOPICOS_REASON_2("[action][REMOVE_USER][userName][Ambrosio][guildName][Topicos de Apicultura][reason][não quero mais ouvi-lo]"),
    KICK_GUSTAVO_IMSERVER_REASON_1("[action][REMOVE_USER][userName][Gustavo][guildName][IMServer][reason][já me esta a irritar]"),
    KICK_GUSTAVO_IMSERVER_REASON_2("[action][REMOVE_USER][userName][Gustavo][guildName][IMServer][reason][não quero mais ouvi-lo]"),
    KICK_GUSTAVO_TOPICOS_REASON_1("[action][REMOVE_USER][userName][Gustavo][guildName][Topicos de Apicultura][reason][já me esta a irritar]"),
    KICK_GUSTAVO_TOPICOS_REASON_2("[action][REMOVE_USER][userName][Gustavo][guildName][Topicos de Apicultura][reason][não quero mais ouvi-lo]"),
    
    BAN_MATOS("[action][BAN_USER][userName][Matos]"),
    BAN_AMBROSIO("[action][BAN_USER][userName][Ambrosio]"),
    BAN_GUSTAVO("[action][BAN_USER][userName][Gustavo]"),
    BAN_MATOS_IMSERVER("[action][BAN_USER][userName][Matos][guildName][IMServer]"),
    BAN_MATOS_TOPICOS("[action][BAN_USER][userName][Matos][guildName][Topicos de Apicultura]"),
    BAN_AMBROSIO_IMSERVER("[action][BAN_USER][userName][Ambrosio][guildName][IMServer]"),
    BAN_AMBROSIO_TOPICOS("[action][BAN_USER][userName][Ambrosio][guildName][Topicos de Apicultura]"),
    BAN_GUSTAVO_IMSERVER("[action][BAN_USER][userName][Gustavo][guildName][IMServer]"),
    BAN_GUSTAVO_TOPICOS("[action][BAN_USER][userName][Gustavo][guildName][Topicos de Apicultura]"),
    BAN_MATOS_REASON_1("[action][BAN_USER][userName][Matos][reason][já me esta a irritar]"),
    BAN_MATOS_REASON_2("[action][BAN_USER][userName][Matos][reason][não quero mais ouvi-lo]"),
    BAN_AMBROSIO_REASON_1("[action][BAN_USER][userName][Ambrosio][reason][já me esta a irritar]"),
    BAN_AMBROSIO_REASON_2("[action][BAN_USER][userName][Ambrosio][reason][não quero mais ouvi-lo]"),
    BAN_GUSTAVO_REASON_1("[action][BAN_USER][userName][Gustavo][reason][já me esta a irritar]"),
    BAN_GUSTAVO_REASON_2("[action][BAN_USER][userName][Gustavo][reason][não quero mais ouvi-lo]"),
    BAN_MATOS_IMSERVER_REASON_1("[action][BAN_USER][userName][Matos][guildName][IMServer][reason][já me esta a irritar]"),
    BAN_MATOS_IMSERVER_REASON_2("[action][BAN_USER][userName][Matos][guildName][IMServer][reason][não quero mais ouvi-lo]"),
    BAN_MATOS_TOPICOS_REASON_1("[action][BAN_USER][userName][Matos][guildName][Topicos de Apicultura][reason][já me esta a irritar]"),
    BAN_MATOS_TOPICOS_REASON_2("[action][BAN_USER][userName][Matos][guildName][Topicos de Apicultura][reason][não quero mais ouvi-lo]"),
    BAN_AMBROSIO_IMSERVER_REASON_1("[action][BAN_USER][userName][Ambrosio][guildName][IMServer][reason][já me esta a irritar]"),
    BAN_AMBROSIO_IMSERVER_REASON_2("[action][BAN_USER][userName][Ambrosio][guildName][IMServer][reason][não quero mais ouvi-lo]"),
    BAN_AMBROSIO_TOPICOS_REASON_1("[action][BAN_USER][userName][Ambrosio][guildName][Topicos de Apicultura][reason][já me esta a irritar]"),
    BAN_AMBROSIO_TOPICOS_REASON_2("[action][BAN_USER][userName][Ambrosio][guildName][Topicos de Apicultura][reason][não quero mais ouvi-lo]"),
    BAN_GUSTAVO_IMSERVER_REASON_1("[action][BAN_USER][userName][Gustavo][guildName][IMServer][reason][já me esta a irritar]"),
    BAN_GUSTAVO_IMSERVER_REASON_2("[action][BAN_USER][userName][Gustavo][guildName][IMServer][reason][não quero mais ouvi-lo]"),
    BAN_GUSTAVO_TOPICOS_REASON_1("[action][BAN_USER][userName][Gustavo][guildName][Topicos de Apicultura][reason][já me esta a irritar]"),
    BAN_GUSTAVO_TOPICOS_REASON_2("[action][BAN_USER][userName][Gustavo][guildName][Topicos de Apicultura][reason][não quero mais ouvi-lo]"),

    DELETE_MESSAGE_GERAL("[action][DELETE_LAST_MESSAGE][channelName][geral]"),
    DELETE_MESSAGE_TESTE("[action][DELETE_LAST_MESSAGE][channelName][teste]"),
    DELETE_MESSAGE_SUECA("[action][DELETE_LAST_MESSAGE][channelName][sueca]"),
    DELETE_MESSAGE_GERAL_IMSERVER("[action][DELETE_LAST_MESSAGE][channelName][geral][guildName][IMServer]"),
    DELETE_MESSAGE_TESTE_IMSERVER("[action][DELETE_LAST_MESSAGE][channelName][teste][guildName][IMServer]"),
    DELETE_MESSAGE_SUECA_IMSERVER("[action][DELETE_LAST_MESSAGE][channelName][sueca][guildName][IMServer]"),
    DELETE_MESSAGE_GERAL_TOPICOS("[action][DELETE_LAST_MESSAGE][channelName][geral][guildName][Topicos de Apicultura]"),
    DELETE_MESSAGE_TESTE_TOPICOS("[action][DELETE_LAST_MESSAGE][channelName][teste][guildName][Topicos de Apicultura]"),
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
