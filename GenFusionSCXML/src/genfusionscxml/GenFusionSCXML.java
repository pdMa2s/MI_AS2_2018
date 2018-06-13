/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package genfusionscxml;

import java.io.IOException;
import scxmlgen.Fusion.FusionGenerator;
import scxmlgen.Modalities.Output;
import scxmlgen.Modalities.Speech;
import scxmlgen.Modalities.SecondMod;

/**
 *
 * @author nunof
 */
public class GenFusionSCXML {

    /**
     * @param args the command line arguments
     */
    public static void main(String[] args) throws IOException {

    FusionGenerator fg = new FusionGenerator();
    
    fg.Single(SecondMod.DELETE_MESSAGE_CHANNEL_TEST_IMSERVER, Output.DELETE_MESSAGE_TESTE_IMSERVER);
    fg.Single(SecondMod.DELETE_MESSAGE_CHANNEL_SUECA_IMSERVER, Output.DELETE_MESSAGE_SUECA_IMSERVER);
    fg.Single(SecondMod.DELETE_MESSAGE_CHANNEL_GERAL_IMSERVER, Output.DELETE_MESSAGE_GERAL_IMSERVER);
    
    fg.Single(SecondMod.KICK_USER_MATOS_IMSERVER, Output.KICK_MATOS_IMSERVER);
    fg.Single(SecondMod.KICK_USER_AMBROSIO_IMSERVER, Output.KICK_AMBROSIO_IMSERVER);
    fg.Single(SecondMod.KICK_USER_GUSTAVO_IMSERVER, Output.KICK_GUSTAVO_IMSERVER);
    
    fg.Single(SecondMod.BAN_USER_MATOS_IMSERVER, Output.BAN_MATOS_IMSERVER);
    fg.Single(SecondMod.BAN_USER_AMBROSIO_IMSERVER, Output.BAN_AMBROSIO_IMSERVER);
    fg.Single(SecondMod.BAN_USER_GUSTAVO_IMSERVER, Output.BAN_GUSTAVO_IMSERVER);
    
    fg.Single(SecondMod.DELETE_MESSAGE_CHANNEL_TEST_TOPICOS, Output.DELETE_MESSAGE_TESTE_TOPICOS);
    fg.Single(SecondMod.DELETE_MESSAGE_CHANNEL_GERAL_IMSERVER, Output.DELETE_MESSAGE_GERAL_TOPICOS);
    
    fg.Single(SecondMod.KICK_USER_MATOS_TOPICOS, Output.KICK_MATOS_TOPICOS);
    fg.Single(SecondMod.KICK_USER_AMBROSIO_TOPICOS, Output.KICK_AMBROSIO_TOPICOS);
    fg.Single(SecondMod.KICK_USER_GUSTAVO_TOPICOS, Output.KICK_GUSTAVO_TOPICOS);
    
    fg.Single(SecondMod.BAN_USER_MATOS_TOPICOS, Output.BAN_MATOS_TOPICOS);
    fg.Single(SecondMod.BAN_USER_AMBROSIO_TOPICOS, Output.BAN_AMBROSIO_TOPICOS);
    fg.Single(SecondMod.BAN_USER_GUSTAVO_TOPICOS, Output.BAN_GUSTAVO_TOPICOS);
    
    fg.Redundancy(Speech.MUTE_AMBROSIO, SecondMod.MUTE_AMBROSIO, Output.MUTE_AMBROSIO);
    fg.Redundancy(Speech.MUTE_AMBROSIO_IMSERVER, SecondMod.MUTE_AMBROSIO_IMSERVER, Output.MUTE_AMBROSIO_IMSERVER);
    fg.Redundancy(Speech.MUTE_AMBROSIO_TOPICOS, SecondMod.MUTE_AMBROSIO_TOPICOS, Output.MUTE_AMBROSIO_TOPICOS);
    fg.Redundancy(Speech.DEAF_AMBROSIO, SecondMod.DEAF_AMBROSIO, Output.DEAF_AMBROSIO);
    fg.Redundancy(Speech.DEAF_AMBROSIO_IMSERVER, SecondMod.DEAF_AMBROSIO_IMSERVER, Output.DEAF_AMBROSIO_IMSERVER);
    fg.Redundancy(Speech.DEAF_AMBROSIO_TOPICOS, SecondMod.DEAF_AMBROSIO_TOPICOS, Output.DEAF_AMBROSIO_TOPICOS);
    fg.Redundancy(Speech.MUTE_GUSTAVO, SecondMod.MUTE_GUSTAVO, Output.MUTE_GUSTAVO);
    fg.Redundancy(Speech.MUTE_GUSTAVO_IMSERVER, SecondMod.MUTE_GUSTAVO_IMSERVER, Output.MUTE_GUSTAVO_IMSERVER);
    fg.Redundancy(Speech.MUTE_GUSTAVO_TOPICOS, SecondMod.MUTE_GUSTAVO_TOPICOS, Output.MUTE_GUSTAVO_TOPICOS);
    fg.Redundancy(Speech.DEAF_GUSTAVO, SecondMod.DEAF_GUSTAVO, Output.DEAF_GUSTAVO);
    fg.Redundancy(Speech.DEAF_GUSTAVO_IMSERVER, SecondMod.DEAF_GUSTAVO_IMSERVER, Output.DEAF_GUSTAVO_IMSERVER);
    fg.Redundancy(Speech.DEAF_GUSTAVO_TOPICOS, SecondMod.DEAF_GUSTAVO_TOPICOS, Output.DEAF_GUSTAVO_TOPICOS);
    fg.Redundancy(Speech.MUTE_MATOS, SecondMod.MUTE_MATOS, Output.MUTE_MATOS);
    fg.Redundancy(Speech.MUTE_MATOS_IMSERVER, SecondMod.MUTE_MATOS_IMSERVER, Output.MUTE_MATOS_IMSERVER);
    fg.Redundancy(Speech.MUTE_MATOS_TOPICOS, SecondMod.MUTE_MATOS_TOPICOS, Output.MUTE_MATOS_TOPICOS);
    fg.Redundancy(Speech.DEAF_MATOS, SecondMod.DEAF_MATOS, Output.DEAF_MATOS);
    fg.Redundancy(Speech.DEAF_MATOS_IMSERVER, SecondMod.DEAF_MATOS_IMSERVER, Output.DEAF_MATOS_IMSERVER);
    fg.Redundancy(Speech.DEAF_MATOS_TOPICOS, SecondMod.DEAF_MATOS_TOPICOS, Output.DEAF_MATOS_TOPICOS);
    fg.Redundancy(Speech.SELF_MUTE, SecondMod.SELF_MUTE, Output.SELF_MUTE);
    fg.Redundancy(Speech.SELF_MUTE_IMSERVER, SecondMod.SELF_MUTE_IMSERVER, Output.SELF_MUTE_IMSERVER);
    fg.Redundancy(Speech.SELF_MUTE_TOPICOS, SecondMod.SELF_MUTE_TOPICOS, Output.SELF_MUTE_TOPICOS);
    fg.Redundancy(Speech.SELF_DEAF, SecondMod.SELF_DEAF, Output.SELF_DEAF);
    fg.Redundancy(Speech.SELF_DEAF_IMSERVER, SecondMod.SELF_DEAF_IMSERVER, Output.SELF_DEAF_IMSERVER);
    fg.Redundancy(Speech.SELF_DEAF_TOPICOS, SecondMod.SELF_DEAF_TOPICOS, Output.SELF_DEAF_TOPICOS);
    
    fg.Complementary(Speech.USER_AMBROSIO, SecondMod.KICK_USER, Output.KICK_AMBROSIO);
    fg.Complementary(Speech.USER_AMBROSIO_IMSERVER, SecondMod.KICK_USER, Output.KICK_AMBROSIO_IMSERVER);
    fg.Complementary(Speech.USER_AMBROSIO, SecondMod.KICK_USER_IMSERVER, Output.KICK_AMBROSIO_IMSERVER);
    fg.Complementary(Speech.USER_AMBROSIO_IMSERVER, SecondMod.KICK_USER_IMSERVER, Output.KICK_AMBROSIO_IMSERVER);
    fg.Complementary(Speech.USER_AMBROSIO_TOPICOS, SecondMod.KICK_USER, Output.KICK_AMBROSIO_TOPICOS);
    fg.Complementary(Speech.USER_AMBROSIO, SecondMod.KICK_USER_TOPICOS, Output.KICK_AMBROSIO_TOPICOS);
    fg.Complementary(Speech.USER_AMBROSIO_TOPICOS, SecondMod.KICK_USER_TOPICOS, Output.KICK_AMBROSIO_TOPICOS);
    fg.Complementary(Speech.USER_GUSTAVO, SecondMod.KICK_USER, Output.KICK_GUSTAVO);
    fg.Complementary(Speech.USER_GUSTAVO_IMSERVER, SecondMod.KICK_USER, Output.KICK_GUSTAVO_IMSERVER);
    fg.Complementary(Speech.USER_GUSTAVO, SecondMod.KICK_USER_IMSERVER, Output.KICK_GUSTAVO_IMSERVER);
    fg.Complementary(Speech.USER_GUSTAVO_IMSERVER, SecondMod.KICK_USER_IMSERVER, Output.KICK_GUSTAVO_IMSERVER);
    fg.Complementary(Speech.USER_GUSTAVO_TOPICOS, SecondMod.KICK_USER, Output.KICK_GUSTAVO_TOPICOS);
    fg.Complementary(Speech.USER_GUSTAVO, SecondMod.KICK_USER_TOPICOS, Output.KICK_GUSTAVO_TOPICOS);
    fg.Complementary(Speech.USER_GUSTAVO_TOPICOS, SecondMod.KICK_USER_TOPICOS, Output.KICK_GUSTAVO_TOPICOS);
    fg.Complementary(Speech.USER_MATOS, SecondMod.KICK_USER, Output.KICK_MATOS);
    fg.Complementary(Speech.USER_MATOS_IMSERVER, SecondMod.KICK_USER, Output.KICK_MATOS_IMSERVER);
    fg.Complementary(Speech.USER_MATOS, SecondMod.KICK_USER_IMSERVER, Output.KICK_MATOS_IMSERVER);
    fg.Complementary(Speech.USER_MATOS_IMSERVER, SecondMod.KICK_USER_IMSERVER, Output.KICK_MATOS_IMSERVER);
    fg.Complementary(Speech.USER_MATOS_TOPICOS, SecondMod.KICK_USER, Output.KICK_MATOS_TOPICOS);
    fg.Complementary(Speech.USER_MATOS, SecondMod.KICK_USER_TOPICOS, Output.KICK_MATOS_TOPICOS);
    fg.Complementary(Speech.USER_MATOS_TOPICOS, SecondMod.KICK_USER_TOPICOS, Output.KICK_MATOS_TOPICOS);
    
    fg.Complementary(Speech.USER_AMBROSIO, SecondMod.BAN_USER, Output.BAN_AMBROSIO);
    fg.Complementary(Speech.USER_AMBROSIO_IMSERVER, SecondMod.BAN_USER, Output.BAN_AMBROSIO_IMSERVER);
    fg.Complementary(Speech.USER_AMBROSIO, SecondMod.BAN_USER_IMSERVER, Output.BAN_AMBROSIO_IMSERVER);
    fg.Complementary(Speech.USER_AMBROSIO_IMSERVER, SecondMod.BAN_USER_IMSERVER, Output.BAN_AMBROSIO_IMSERVER);
    fg.Complementary(Speech.USER_AMBROSIO_TOPICOS, SecondMod.BAN_USER, Output.BAN_AMBROSIO_TOPICOS);
    fg.Complementary(Speech.USER_AMBROSIO, SecondMod.BAN_USER_TOPICOS, Output.BAN_AMBROSIO_TOPICOS);
    fg.Complementary(Speech.USER_AMBROSIO_TOPICOS, SecondMod.BAN_USER_TOPICOS, Output.BAN_AMBROSIO_TOPICOS);
    fg.Complementary(Speech.USER_GUSTAVO, SecondMod.BAN_USER, Output.BAN_GUSTAVO);
    fg.Complementary(Speech.USER_GUSTAVO_IMSERVER, SecondMod.BAN_USER, Output.BAN_GUSTAVO_IMSERVER);
    fg.Complementary(Speech.USER_GUSTAVO, SecondMod.BAN_USER_IMSERVER, Output.BAN_GUSTAVO_IMSERVER);
    fg.Complementary(Speech.USER_GUSTAVO_IMSERVER, SecondMod.BAN_USER_IMSERVER, Output.BAN_GUSTAVO_IMSERVER);
    fg.Complementary(Speech.USER_GUSTAVO_TOPICOS, SecondMod.BAN_USER, Output.BAN_GUSTAVO_TOPICOS);
    fg.Complementary(Speech.USER_GUSTAVO, SecondMod.BAN_USER_TOPICOS, Output.BAN_GUSTAVO_TOPICOS);
    fg.Complementary(Speech.USER_GUSTAVO_TOPICOS, SecondMod.BAN_USER_TOPICOS, Output.BAN_GUSTAVO_TOPICOS);
    fg.Complementary(Speech.USER_MATOS, SecondMod.BAN_USER, Output.BAN_MATOS);
    fg.Complementary(Speech.USER_MATOS_IMSERVER, SecondMod.BAN_USER, Output.BAN_MATOS_IMSERVER);
    fg.Complementary(Speech.USER_MATOS, SecondMod.BAN_USER_IMSERVER, Output.BAN_MATOS_IMSERVER);
    fg.Complementary(Speech.USER_MATOS_IMSERVER, SecondMod.BAN_USER_IMSERVER, Output.BAN_MATOS_IMSERVER);
    fg.Complementary(Speech.USER_MATOS_TOPICOS, SecondMod.BAN_USER, Output.BAN_MATOS_TOPICOS);
    fg.Complementary(Speech.USER_MATOS, SecondMod.BAN_USER_TOPICOS, Output.BAN_MATOS_TOPICOS);
    fg.Complementary(Speech.USER_MATOS_TOPICOS, SecondMod.BAN_USER_TOPICOS, Output.BAN_MATOS_TOPICOS);
    
    fg.Complementary(Speech.CHANNEL_GERAL, SecondMod.DELETE_MESSAGE, Output.DELETE_MESSAGE_GERAL);
    fg.Complementary(Speech.CHANNEL_GERAL_IMSERVER, SecondMod.DELETE_MESSAGE, Output.DELETE_MESSAGE_GERAL_IMSERVER);
    fg.Complementary(Speech.CHANNEL_GERAL, SecondMod.DELETE_MESSAGE_IMSERVER, Output.DELETE_MESSAGE_GERAL_IMSERVER);
    fg.Complementary(Speech.CHANNEL_GERAL_IMSERVER, SecondMod.DELETE_MESSAGE_IMSERVER, Output.DELETE_MESSAGE_GERAL_IMSERVER);
    fg.Complementary(Speech.CHANNEL_GERAL_TOPICOS, SecondMod.DELETE_MESSAGE, Output.DELETE_MESSAGE_GERAL_TOPICOS);
    fg.Complementary(Speech.CHANNEL_GERAL, SecondMod.DELETE_MESSAGE_TOPICOS, Output.DELETE_MESSAGE_GERAL_TOPICOS);
    fg.Complementary(Speech.CHANNEL_GERAL_TOPICOS, SecondMod.DELETE_MESSAGE_TOPICOS, Output.DELETE_MESSAGE_GERAL_TOPICOS);
    fg.Complementary(Speech.CHANNEL_TESTE, SecondMod.DELETE_MESSAGE, Output.DELETE_MESSAGE_TESTE);
    fg.Complementary(Speech.CHANNEL_TESTE_IMSERVER, SecondMod.DELETE_MESSAGE, Output.DELETE_MESSAGE_TESTE_IMSERVER);
    fg.Complementary(Speech.CHANNEL_TESTE, SecondMod.DELETE_MESSAGE_IMSERVER, Output.DELETE_MESSAGE_TESTE_IMSERVER);
    fg.Complementary(Speech.CHANNEL_TESTE_IMSERVER, SecondMod.DELETE_MESSAGE_IMSERVER, Output.DELETE_MESSAGE_TESTE_IMSERVER);
    fg.Complementary(Speech.CHANNEL_TESTE_TOPICOS, SecondMod.DELETE_MESSAGE, Output.DELETE_MESSAGE_TESTE_TOPICOS);
    fg.Complementary(Speech.CHANNEL_TESTE, SecondMod.DELETE_MESSAGE_TOPICOS, Output.DELETE_MESSAGE_TESTE_TOPICOS);
    fg.Complementary(Speech.CHANNEL_TESTE_TOPICOS, SecondMod.DELETE_MESSAGE_TOPICOS, Output.DELETE_MESSAGE_TESTE_TOPICOS);
    fg.Complementary(Speech.CHANNEL_SUECA, SecondMod.DELETE_MESSAGE, Output.DELETE_MESSAGE_SUECA);
    fg.Complementary(Speech.CHANNEL_SUECA_IMSERVER, SecondMod.DELETE_MESSAGE, Output.DELETE_MESSAGE_SUECA_IMSERVER);
    fg.Complementary(Speech.CHANNEL_SUECA, SecondMod.DELETE_MESSAGE_IMSERVER, Output.DELETE_MESSAGE_SUECA_IMSERVER);
    fg.Complementary(Speech.CHANNEL_SUECA_IMSERVER, SecondMod.DELETE_MESSAGE_IMSERVER, Output.DELETE_MESSAGE_SUECA_IMSERVER);
    
    fg.Complementary(Speech.USER_AMBROSIO, SecondMod.MUTE_USER, Output.MUTE_AMBROSIO);
    fg.Complementary(Speech.USER_AMBROSIO_IMSERVER, SecondMod.MUTE_USER, Output.MUTE_AMBROSIO_IMSERVER);
    fg.Complementary(Speech.USER_AMBROSIO, SecondMod.MUTE_USER_IMSERVER, Output.MUTE_AMBROSIO_IMSERVER);
    fg.Complementary(Speech.USER_AMBROSIO_IMSERVER, SecondMod.MUTE_USER_IMSERVER, Output.MUTE_AMBROSIO_IMSERVER);
    fg.Complementary(Speech.USER_AMBROSIO_TOPICOS, SecondMod.MUTE_USER, Output.MUTE_AMBROSIO_TOPICOS);
    fg.Complementary(Speech.USER_AMBROSIO, SecondMod.MUTE_USER_TOPICOS, Output.MUTE_AMBROSIO_TOPICOS);
    fg.Complementary(Speech.USER_AMBROSIO_TOPICOS, SecondMod.MUTE_USER_TOPICOS, Output.MUTE_AMBROSIO_TOPICOS);
    fg.Complementary(Speech.USER_AMBROSIO, SecondMod.DEAF_USER, Output.DEAF_AMBROSIO);
    fg.Complementary(Speech.USER_AMBROSIO_IMSERVER, SecondMod.DEAF_USER, Output.DEAF_AMBROSIO_IMSERVER);
    fg.Complementary(Speech.USER_AMBROSIO, SecondMod.DEAF_USER_IMSERVER, Output.DEAF_AMBROSIO_IMSERVER);
    fg.Complementary(Speech.USER_AMBROSIO_IMSERVER, SecondMod.DEAF_USER_IMSERVER, Output.DEAF_AMBROSIO_IMSERVER);
    fg.Complementary(Speech.USER_AMBROSIO_TOPICOS, SecondMod.DEAF_USER, Output.DEAF_AMBROSIO_TOPICOS);
    fg.Complementary(Speech.USER_AMBROSIO, SecondMod.DEAF_USER_TOPICOS, Output.DEAF_AMBROSIO_TOPICOS);
    fg.Complementary(Speech.USER_AMBROSIO_TOPICOS, SecondMod.DEAF_USER_TOPICOS, Output.DEAF_AMBROSIO_TOPICOS);
    fg.Complementary(Speech.USER_GUSTAVO, SecondMod.MUTE_USER, Output.MUTE_GUSTAVO);
    fg.Complementary(Speech.USER_GUSTAVO_IMSERVER, SecondMod.MUTE_USER, Output.MUTE_GUSTAVO_IMSERVER);
    fg.Complementary(Speech.USER_GUSTAVO, SecondMod.MUTE_USER_IMSERVER, Output.MUTE_GUSTAVO_IMSERVER);
    fg.Complementary(Speech.USER_GUSTAVO_IMSERVER, SecondMod.MUTE_USER_IMSERVER, Output.MUTE_GUSTAVO_IMSERVER);
    fg.Complementary(Speech.USER_GUSTAVO_TOPICOS, SecondMod.MUTE_USER, Output.MUTE_GUSTAVO_TOPICOS);
    fg.Complementary(Speech.USER_GUSTAVO, SecondMod.MUTE_USER_TOPICOS, Output.MUTE_GUSTAVO_TOPICOS);
    fg.Complementary(Speech.USER_GUSTAVO_TOPICOS, SecondMod.MUTE_USER_TOPICOS, Output.MUTE_GUSTAVO_TOPICOS);
    fg.Complementary(Speech.USER_GUSTAVO, SecondMod.DEAF_USER, Output.DEAF_GUSTAVO);
    fg.Complementary(Speech.USER_GUSTAVO_IMSERVER, SecondMod.DEAF_USER, Output.DEAF_GUSTAVO_IMSERVER);
    fg.Complementary(Speech.USER_GUSTAVO, SecondMod.DEAF_USER_IMSERVER, Output.DEAF_GUSTAVO_IMSERVER);
    fg.Complementary(Speech.USER_GUSTAVO_IMSERVER, SecondMod.DEAF_USER_IMSERVER, Output.DEAF_GUSTAVO_IMSERVER);
    fg.Complementary(Speech.USER_GUSTAVO_TOPICOS, SecondMod.DEAF_USER, Output.DEAF_GUSTAVO_TOPICOS);
    fg.Complementary(Speech.USER_GUSTAVO, SecondMod.DEAF_USER_TOPICOS, Output.DEAF_GUSTAVO_TOPICOS);
    fg.Complementary(Speech.USER_GUSTAVO_TOPICOS, SecondMod.DEAF_USER_TOPICOS, Output.DEAF_GUSTAVO_TOPICOS);
    fg.Complementary(Speech.USER_MATOS, SecondMod.MUTE_USER, Output.MUTE_MATOS);
    fg.Complementary(Speech.USER_MATOS_IMSERVER, SecondMod.MUTE_USER, Output.MUTE_MATOS_IMSERVER);
    fg.Complementary(Speech.USER_MATOS, SecondMod.MUTE_USER_IMSERVER, Output.MUTE_MATOS_IMSERVER);
    fg.Complementary(Speech.USER_MATOS_IMSERVER, SecondMod.MUTE_USER_IMSERVER, Output.MUTE_MATOS_IMSERVER);
    fg.Complementary(Speech.USER_MATOS_TOPICOS, SecondMod.MUTE_USER, Output.MUTE_MATOS_TOPICOS);
    fg.Complementary(Speech.USER_MATOS, SecondMod.MUTE_USER_TOPICOS, Output.MUTE_MATOS_TOPICOS);
    fg.Complementary(Speech.USER_MATOS_TOPICOS, SecondMod.MUTE_USER_TOPICOS, Output.MUTE_MATOS_TOPICOS);
    fg.Complementary(Speech.USER_MATOS, SecondMod.DEAF_USER, Output.DEAF_MATOS);
    fg.Complementary(Speech.USER_MATOS_IMSERVER, SecondMod.DEAF_USER, Output.DEAF_MATOS_IMSERVER);
    fg.Complementary(Speech.USER_MATOS, SecondMod.DEAF_USER_IMSERVER, Output.DEAF_MATOS_IMSERVER);
    fg.Complementary(Speech.USER_MATOS_IMSERVER, SecondMod.DEAF_USER_IMSERVER, Output.DEAF_MATOS_IMSERVER);
    fg.Complementary(Speech.USER_MATOS_TOPICOS, SecondMod.DEAF_USER, Output.DEAF_MATOS_TOPICOS);
    fg.Complementary(Speech.USER_MATOS, SecondMod.DEAF_USER_TOPICOS, Output.DEAF_MATOS_TOPICOS);
    fg.Complementary(Speech.USER_MATOS_TOPICOS, SecondMod.DEAF_USER_TOPICOS, Output.DEAF_MATOS_TOPICOS);
    
    /*fg.Sequence(Speech.SQUARE, SecondMod.RED, Output.SQUARE_RED);
    fg.Sequence(Speech.SQUARE, SecondMod.BLUE, Output.SQUARE_BLUE);
    fg.Sequence(Speech.SQUARE, SecondMod.YELLOW, Output.SQUARE_YELLOW);
    fg.Complementary(Speech.TRIANGLE, SecondMod.RED, Output.TRIANGLE_RED);
    fg.Complementary(Speech.TRIANGLE, SecondMod.BLUE, Output.TRIANGLE_BLUE);
    fg.Complementary(Speech.TRIANGLE, SecondMod.YELLOW, Output.TRIANGLE_YELLOW);
    fg.Complementary(Speech.CIRCLE, SecondMod.RED, Output.CIRCLE_RED);
    fg.Complementary(Speech.CIRCLE, SecondMod.BLUE, Output.CIRCLE_BLUE);
    fg.Complementary(Speech.CIRCLE, SecondMod.YELLOW, Output.CIRCLE_YELLOW);*/
    
    //fg.Single(Speech.CIRCLE, Output.CIRCLE);  //EXAMPLE
    //fg.Redundancy(Speech.CIRCLE, SecondMod.CIRCLE, Output.CIRCLE);  //EXAMPLE
    
    fg.Build("fusion.scxml");
        
        
    }
    
}
