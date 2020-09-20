import 'package:capturacognitivajaveriana/app/data/services/auth_service.dart';
import 'package:capturacognitivajaveriana/app/routes/app_pages.dart';
import 'package:get/get.dart';
import 'package:get/utils.dart';
import 'package:flutter/material.dart';

class ResetPage extends StatefulWidget {
  @override
  _ResetPageState createState() => _ResetPageState();
}

class _ResetPageState extends State<ResetPage> {
  final AuthService authService = AuthService();
  final GlobalKey<FormState> _formKey = GlobalKey<FormState>();
  TextEditingController _emailController = TextEditingController();
  bool loading = false;

  @override
  Widget build(BuildContext context) {
    final ThemeData theme = Theme.of(context);

    return SafeArea(
      child: Scaffold(
        backgroundColor: theme.primaryColor,
        appBar: AppBar(
          elevation: 0.0,
          backgroundColor: Colors.transparent,
        ),
        body: Container(
          constraints: BoxConstraints.expand(),
          decoration: BoxDecoration(
            color: Colors.white,
            borderRadius: BorderRadius.only(
              topLeft: Radius.circular(20.0),
              topRight: Radius.circular(20.0),
            ),
          ),
          child: SingleChildScrollView(
            padding: EdgeInsets.all(12.0),
            child: Form(
              key: _formKey,
              child: Column(
                mainAxisSize: MainAxisSize.max,
                mainAxisAlignment: MainAxisAlignment.center,
                children: [
                  SizedBox(
                    height: 20.0,
                  ),
                  Text(
                    "Reset password",
                    style: Theme.of(context)
                        .textTheme
                        .headline5
                        .apply(fontWeightDelta: 900),
                  ),
                  ListTile(
                    leading: Icon(Icons.email),
                    title: TextFormField(
                      controller: _emailController,
                      keyboardType: TextInputType.emailAddress,
                      decoration:
                          InputDecoration(isDense: true, labelText: "Correo"),
                      validator: (value) {
                        if (GetUtils.isEmail(value)) {
                          return null;
                        }
                        return "Ingrese un correo válido";
                      },
                    ),
                  ),
                  Container(
                      width: double.infinity,
                      child: FlatButton.icon(
                        icon: loading
                            ? SizedBox(
                                width: 20.0,
                                height: 20.0,
                                child: CircularProgressIndicator(
                                  valueColor: AlwaysStoppedAnimation<Color>(
                                      Colors.white),
                                  strokeWidth: 3.0,
                                ))
                            : SizedBox.shrink(),
                        color: theme.accentColor,
                        label: Text(
                          "CAMBIAR CONTRASEÑA",
                          style: TextStyle(fontWeight: FontWeight.bold),
                        ),
                        onPressed: () async {
                          if (_formKey.currentState.validate()) {
                            setState(() {
                              loading = true;
                            });
                            var resp = await this
                                .authService
                                .resetPassword(_emailController.text);
                            setState(() {
                              loading = false;
                            });

                            print(resp.toString());
                            if (resp["response"]) {
                              Get.offAndToNamed(Routes.LOGIN);
                            } else {
                              Get.defaultDialog(
                                title:
                                    "Error de restablecimiento de contraseña.",
                                content: Container(
                                  child: Text(
                                    resp["message"],
                                  ),
                                ),
                              );
                            }
                          }
                        },
                      )),
                ],
              ),
            ),
          ),
        ),
      ),
    );
  }
}
