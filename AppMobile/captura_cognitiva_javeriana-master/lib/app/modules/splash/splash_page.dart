import 'package:capturacognitivajaveriana/app/modules/splash/splash_controller.dart';
import 'package:flutter/material.dart';
import 'package:get/state_manager.dart';

class SplashPage extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return GetBuilder<SplashController>(
        init: SplashController(),
        builder: (controller) {
          return Scaffold(
            body: Container(
              constraints: BoxConstraints.expand(),
              child: Column(
                mainAxisSize: MainAxisSize.max,
                mainAxisAlignment: MainAxisAlignment.center,
                children: [
                  Text(
                    "Captura Cognitiva",
                    style: TextStyle(
                      fontWeight: FontWeight.bold,
                      fontSize: 24.0,
                    ),
                  ),
                  CircularProgressIndicator(),
                ],
              ),
            ),
          );
        });
  }
}
