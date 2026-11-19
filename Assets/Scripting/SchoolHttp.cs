using System;
using System.Net.Http;
using System.Linq;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

// The school server omits this intermediate CA from its TLS handshake.
// Bundle the official issuer and R46 root for Unity runtimes with an older trust store.
// Source: https://secure.globalsign.com/cacert/gsgccr46alphasslca2025.crt
// Root: https://secure.globalsign.com/cacert/rootr46.crt
public static class SchoolHttp
{
    public static HttpClient CreateClient(byte[] intermediateCertificate, byte[] rootCertificate)
    {
        var handler = new HttpClientHandler();
        handler.ServerCertificateCustomValidationCallback = (request, certificate, chain, errors) =>
        {
            if (errors == SslPolicyErrors.None) return true;
            // Never waive a hostname mismatch, missing certificate or non-school host.
            if (errors != SslPolicyErrors.RemoteCertificateChainErrors || certificate == null
                || request.RequestUri == null || request.RequestUri.Scheme != "https"
                || !request.RequestUri.Host.Equals("www.htl-salzburg.ac.at", StringComparison.OrdinalIgnoreCase))
                return false;
            using (var issuer = new X509Certificate2(intermediateCertificate))
            using (var root = new X509Certificate2(rootCertificate))
            using (var completeChain = new X509Chain())
            {
                completeChain.ChainPolicy.ExtraStore.Add(issuer);
                completeChain.ChainPolicy.ExtraStore.Add(root);
                completeChain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllowUnknownCertificateAuthority;
                completeChain.ChainPolicy.RevocationMode = X509RevocationMode.NoCheck;
                if (!completeChain.Build(certificate)) return false;
                // Unity's older trust store may omit R46. Trust only this bundled
                // GlobalSign root, and only after validating signatures and dates.
                var elements = completeChain.ChainElements;
                if (elements.Count < 3) return false;
                if (!elements[elements.Count - 1].Certificate.RawData.SequenceEqual(root.RawData)) return false;
                foreach (var entry in completeChain.ChainStatus)
                    if (entry.Status != X509ChainStatusFlags.NoError && entry.Status != X509ChainStatusFlags.UntrustedRoot)
                        return false;
                return true;
            }
        };
        return new HttpClient(handler) { Timeout = TimeSpan.FromSeconds(20) };
    }
}
