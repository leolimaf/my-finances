import { useRef, useState, useEffect } from "react";
import { FaCheck, FaTimes, FaInfoCircle } from "react-icons/fa";
import axios from './api/axios';

const NAME_REGEX = /^[A-z]{3,119}$/;
const EMAIL_REGEX = /^(?=.*[a-z])(?=.*[A-Z]).{8,24}$/; // TODO: criar regex para validar o email
const PWD_REGEX = /^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9]).{8,24}$/;
const REGISTER_URL = '/v1/autenticacao/cadastrar';

const Register = () => {

    const userRef = useRef();
    const errRef = useRef();

    const [name, setName] = useState('');
    const [validName, setValidName] = useState(false);
    const [nameFocus, setNameFocus] = useState(false);

    const [email, setEmail] = useState('');
    const [validEmail, setValidEmail] = useState(false);
    const [emailFocus, setEmailFocus] = useState(false);

    const [pwd, setPwd] = useState('');
    const [validPwd, setValidPwd] = useState(false);
    const [pwdFocus, setPwdFocus] = useState(false);

    const [matchPwd, setMatchPwd] = useState('');
    const [validMatch, setValidMatch] = useState(false);
    const [matchFocus, setMatchFocus] = useState(false);

    const [errMsg, setErrMsg] = useState('');
    const [success, setSuccess] = useState(false);

    useEffect(() => {
        userRef.current.focus();
    }, [])

    useEffect(() => {
        setValidName(NAME_REGEX.test(name));
    }, [name])

    useEffect(() => {
        setValidEmail(EMAIL_REGEX.test(email));
    }, [email])

    useEffect(() => {
        setValidPwd(PWD_REGEX.test(pwd));
        setValidMatch(pwd === matchPwd);
    }, [pwd, matchPwd])

    useEffect(() => {
        setErrMsg('');
    }, [name, email, pwd, matchPwd])

    const handleSubmit = async (e) => {
        e.preventDefault();
        
        const v1 = NAME_REGEX.test(name);
        const v2 = EMAIL_REGEX.test(email);
        const v3 = PWD_REGEX.test(pwd);
        if (!v1 || !v2 || v3) {
            setErrMsg("Entrada inválida");
            return;
        }
        try {
            const response = await axios.post(REGISTER_URL,
                JSON.stringify({ nome: name, email: email, senha: pwd }),
                {
                    headers: { 'Content-Type': 'application/json' },
                    withCredentials: false // TODO: configurar origin
                }
            );
            setSuccess(true);
            setName('');
            setEmail('');
            setPwd('');
            setMatchPwd('');
        } catch (err) {
            if (!err?.response) {
                setErrMsg('Servidor não responde.');
            } else if (err.response?.status === 409) {
                setErrMsg(err.response?.error);
            } else {
                setErrMsg('Falha ao cadastrar usuário.')
            }
            errRef.current.focus();
        }
    }

    return (
        <>
            {success ? (
                    <section>
                        <h1>Login</h1>
                        <p>
                            <a href="#">Acessar</a>
                        </p>
                    </section>
                ) : (
                <section>
                    <p ref={errRef} className={errMsg ? "errmsg" : "offscreen"} aria-live="assertive">{errMsg}</p>
                    <h1>Registro</h1>
                    <form onSubmit={handleSubmit}>
                        <label htmlFor="username">
                            Nome Completo:
                            <FaCheck className={validName ? "valid" : "hide"} />
                            <FaTimes className={validName || !name ? "hide" : "invalid"} />
                        </label>
                        <input
                            type="text"
                            id="username"
                            ref={userRef}
                            autoComplete="off"
                            onChange={(e) => setName(e.target.value)}
                            value={name}
                            required
                            aria-invalid={validName ? "false" : "true"}
                            aria-describedby="uidnote"
                            onFocus={() => setNameFocus(true)}
                            onBlur={() => setNameFocus(false)}
                        />
                        <p id="uidnote" className={nameFocus && name && !validName ? "instructions" : "offscreen"}>
                            <FaInfoCircle />
                            4 a 120 caracteres.<br />
                            Somente letras são permitidas.
                        </p>

                        <label htmlFor="email">
                            Email:
                            <FaCheck className={validEmail ? "valid" : "hide"} />
                            <FaTimes className={validEmail || !email ? "hide" : "invalid"} />
                        </label>
                        <input
                            type="email"
                            id="useremail"
                            ref={userRef}
                            autoComplete="off"
                            onChange={(e) => setEmail(e.target.value)}
                            value={email}
                            required
                            aria-invalid={validEmail ? "false" : "true"}
                            aria-describedby="uidnote"
                            onFocus={() => setEmailFocus(true)}
                            onBlur={() => setEmailFocus(false)}
                        />
                        <p id="uidnote" className={emailFocus && email && !validEmail ? "instructions" : "offscreen"}>
                            <FaInfoCircle />
                            E-mail inválido.
                        </p>

                        <label htmlFor="password">
                            Senha:
                            <FaCheck className={validPwd ? "valid" : "hide"} />
                            <FaTimes className={validPwd || !pwd ? "hide" : "invalid"} />
                        </label>
                        <input
                            type="password"
                            id="password"
                            onChange={(e) => setPwd(e.target.value)}
                            value={pwd}
                            required
                            aria-invalid={validPwd ? "false" : "true"}
                            aria-describedby="pwdnote"
                            onFocus={() => setPwdFocus(true)}
                            onBlur={() => setPwdFocus(false)}
                        />
                        <p id="pwdnote" className={pwdFocus && !validPwd ? "instructions" : "offscreen"}>
                            <FaInfoCircle />
                            8 a 24 caracteres.<br />
                            Deve conter ao menos uma letra minuscula, uma letra maiuscula e um número.<br />
                            Caracteres especiais são permitidos.
                        </p>

                        <label htmlFor="confirm_pwd">
                            Confirmar Senha:
                            <FaCheck className={validMatch && matchPwd ? "valid" : "hide"} />
                            <FaTimes className={validMatch || !matchPwd ? "hide" : "invalid"} />
                        </label>
                        <input
                            type="password"
                            id="confirm_pwd"
                            onChange={(e) => setMatchPwd(e.target.value)}
                            value={matchPwd}
                            required
                            aria-invalid={validMatch ? "false" : "true"}
                            aria-describedby="confirmnote"
                            onFocus={() => setMatchFocus(true)}
                            onBlur={() => setMatchFocus(false)}
                        />
                        <p id="confirmnote" className={matchFocus && !validMatch ? "instructions" : "offscreen"}>
                            <FaInfoCircle />
                            As senhas devem ser iguais.
                        </p>
                        
                        <button disabled={!validName || !validPwd || !validMatch ? true : false}>Cadastrar</button>
                    </form>

                    <p>
                        Já possui uma conta?<br />
                        <span className="line">
                            {/*TODO: adicionar link da rota*/}
                            <a href="#">Acessar</a>
                        </span>
                    </p>
                </section>
            )}
        </>
    )
}

export default Register